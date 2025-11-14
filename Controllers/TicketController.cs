using ITSupportBE.Api.Data;
using ITSupportBE.Api.DTOs;
using ITSupportBE.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ITSupportBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITSupportDbContext _context; // Sử dụng DbContext của anh

        public TicketsController(ITSupportDbContext context)
        {
            _context = context;
        }

        // POST: api/tickets
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            // Lấy các trạng thái cần thiết
            var newStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "New");
            // (THÊM MỚI) Lấy trạng thái "Wait"
            var waitStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "Wait");

            if (newStatus == null || waitStatus == null)
            {
                return NotFound("Không tìm thấy các trạng thái TicketStatus (New, Wait) trong CSDL.");
            }

            // Tìm kiếm KB (Logic giữ nguyên)
            string searchQuery = dto.Title + " " + dto.Description;
            var keywords = searchQuery.ToLower()
                .Split(new[] { ' ', ',', '.', '?', '!' }, 
                StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .Where(k => k.Length > 2)
                .ToList();
            var allArticles = await _context.KBArticle
                .Where(a => a.IsActive && !a.IsDeleted)
                .ToListAsync();
            var bestMatch = allArticles.Select(article => new { 
                Article = article, 
                MatchCount = keywords.Count(k => article.Title.ToLower().Contains(k)) })
                .Where(x => x.MatchCount > 0)
                .OrderByDescending(x => x.MatchCount)
                .FirstOrDefault();

            var adminUser = await _context.User.FirstOrDefaultAsync(u => u.Role.RoleName == "Admin");
            if (adminUser == null) return NotFound("Tài khoản Admin (dùng để gửi tin nhắn hệ thống) không tồn tại.");

            // Chuẩn bị ticket
            var newTicket = new Ticket
            {
                UserId = dto.StudentId,
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                AssigneeId = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            if (bestMatch != null && bestMatch.MatchCount > 1)
            {
                // TÌM THẤY KB
                // Gán trạng thái là "New" (Chờ SV phản hồi)
                newTicket.StatusId = newStatus.StatusId;
                _context.Ticket.Add(newTicket);
                await _context.SaveChangesAsync();

                // Gửi auto-reply (hướng dẫn + link)
                string huongDanPlainText = bestMatch.Article.Content;
                var autoReply = new TicketReply
                { /* ... (nội dung auto-reply giữ nguyên) ... */
                    TicketId = newTicket.TicketId,
                    UserId = adminUser.UserId,
                    Message = $"[TỰ ĐỘNG] Cảm ơn bạn đã gửi yêu cầu.\n" +
                              $"Hệ thống tìm thấy một bài viết có thể liên quan:\n\n" +
                              $"---[ HƯỚNG DẪN: {bestMatch.Article.Title} ]---\n\n" +
                              $"{huongDanPlainText}\n\n" +
                              $"--------------------------------------------------\n" +
                              $"Để xem bài viết gốc (có hình ảnh), vui lòng truy cập:\n" +
                              $"{bestMatch.Article.OriginalURL}\n\n" +
                              $"(Nếu bài viết này không giải quyết được, hãy kết nối KTV để được hỗ trợ trực tiếp!.)",
                    CreatedAt = DateTime.UtcNow
                };
                _context.TicketReply.Add(autoReply);

                _context.Notification.Add(new Notification { 
                    UserId = dto.StudentId, 
                    TicketId = newTicket.TicketId, 
                    Message = "Hệ thống đã gửi một phản hồi tự động." });
            }
            else
            {
                // KHÔNG TÌM THẤY KB

                // (THAY ĐỔI) Tự động chuyển trạng thái sang "Wait" (Chờ KTV)
                newTicket.StatusId = waitStatus.StatusId;
                _context.Ticket.Add(newTicket);
                await _context.SaveChangesAsync();

                // Ghi lại lịch sử (Hệ thống tự động chuyển)
                _context.TicketHistory.Add(new TicketHistory
                {
                    TicketId = newTicket.TicketId,
                    UserId = adminUser.UserId,
                    Action = "Change Status",
                    OldValue = "New",
                    NewValue = "Wait", // (THAY ĐỔI)
                    ChangedAt = DateTime.UtcNow
                });

                // Gửi tin nhắn "Đang kết nối" cho SV
                var connectReply = new TicketReply
                {
                    TicketId = newTicket.TicketId,
                    UserId = adminUser.UserId,
                    Message = $"[TỰ ĐỘNG] Cảm ơn bạn đã gửi yêu cầu.\n" +
                              $"Hệ thống không tìm thấy hướng dẫn phù hợp.\n\n" +
                              $"Yêu cầu của bạn đang được chuyển đến Kỹ thuật viên (KTV). Vui lòng chờ KTV kết nối.",
                    CreatedAt = DateTime.UtcNow
                };
                _context.TicketReply.Add(connectReply);

                // Thông báo "chuông" cho KTV
                var ktvs = await _context.User.Where(u => u.Role.RoleName == "KTV" && u.IsActive).ToListAsync();
                foreach (var ktv in ktvs)
                {
                    _context.Notification.Add(new Notification
                    {
                        UserId = ktv.UserId,
                        TicketId = newTicket.TicketId,
                        Message = $"Ticket mới #{newTicket.TicketId} ({dto.Title}) đang chờ KTV." // (Thay đổi)
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(newTicket); // Trả về Ticket (cho Frontend)
        }

        // (THAY ĐỔI) API "Request KTV"
        [HttpPost("{id}/request-ktv")]
        public async Task<IActionResult> RequestKtv(int id, [FromBody] RequestKtvDto dto)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null) return NotFound();

            // Lấy trạng thái "Wait"
            var waitStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "Wait");
            if (waitStatus == null) return NotFound("Trạng thái 'Wait' không tồn tại.");

            // (THAY ĐỔI) Chuyển trạng thái Ticket sang "Wait"
            ticket.StatusId = waitStatus.StatusId;
            ticket.UpdatedAt = DateTime.UtcNow;

            // Ghi lịch sử (SV là người bấm)
            _context.TicketHistory.Add(new TicketHistory
            {
                TicketId = id,
                UserId = dto.StudentId,
                Action = "Change Status",
                OldValue = "New",
                NewValue = "Wait", // (THAY ĐỔI)
                ChangedAt = DateTime.UtcNow
            });

            // Gửi tin nhắn hệ thống vào chat
            var adminUser = await _context.User.FirstOrDefaultAsync(u => u.Role.RoleName == "Admin");
            _context.TicketReply.Add(new TicketReply
            {
                TicketId = id,
                UserId = adminUser.UserId,
                Message = "[TỰ ĐỘNG] Yêu cầu đã được chuyển tới KTV. Vui lòng chờ phản hồi.",
                CreatedAt = DateTime.UtcNow
            });

            // Gửi thông báo "chuông" cho TẤT CẢ KTV
            var ktvs = await _context.User.Where(u => u.Role.RoleName == "KTV" && u.IsActive).ToListAsync();
            foreach (var ktv in ktvs)
            {
                _context.Notification.Add(new Notification
                {
                    UserId = ktv.UserId,
                    TicketId = ticket.TicketId,
                    Message = $"Sinh viên cần hỗ trợ TRỰC TIẾP cho ticket #{ticket.TicketId}."
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã yêu cầu KTV. Vui lòng chờ..." });
        }

        // (THAY ĐỔI) API "GetNewTickets" (cho KTV Dashboard)
        // Giờ đây nó chỉ lấy các ticket "Wait"
        [HttpGet("queue/new")]
        public async Task<IActionResult> GetNewTickets()
        {
            var tickets = await _context.Ticket
                .Include(t => t.Requester)
                .Include(t => t.Category)
                // (THAY ĐỔI) Chỉ lấy trạng thái "Wait"
                .Where(t => t.Status.StatusName == "Wait" && t.IsDeleted == false)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new {
                    id = t.TicketId.ToString(),
                    subject = t.Title,
                    requester = t.Requester.FullName,
                    category = t.Category.CategoryName,
                    status = "Wait", // (THAY ĐỔI)
                    updatedAt = t.UpdatedAt.ToString("dd-MM-yyyy")
                })
                .ToListAsync();
            return Ok(tickets);
        }

        // (THÊM MỚI) API cho KTV Tab "My Assigned Tickets"
        [HttpGet("queue/assigned/{ktvId}")]
        public async Task<IActionResult> GetMyAssignedTickets(int ktvId)
        {
            var tickets = await _context.Ticket
                .Include(t => t.Requester)
                .Include(t => t.Category)
                // Lấy ticket "In Progress" VÀ được gán cho KTV này
                .Where(t => t.Status.StatusName == "In Progress"
                            && t.AssigneeId == ktvId
                            && t.IsDeleted == false)
                .OrderByDescending(t => t.UpdatedAt)
                .Select(t => new {
                    id = t.TicketId.ToString(),
                    subject = t.Title,
                    requester = t.Requester.FullName,
                    category = t.Category.CategoryName,
                    status = "In Progress"
                })
                .ToListAsync();
            return Ok(tickets);
        }

        // (THÊM MỚI) API cho KTV Tab "Resolved Tickets"
        [HttpGet("queue/resolved")]
        public async Task<IActionResult> GetResolvedTickets()
        {
            var tickets = await _context.Ticket
                .Include(t => t.Requester)
                .Include(t => t.Category)
                .Where(t => t.Status.StatusName == "Resolved" && t.IsDeleted == false)
                .OrderByDescending(t => t.UpdatedAt)
                .Take(20) // Lấy 20 ticket đã giải quyết gần nhất
                .Select(t => new {
                    id = t.TicketId.ToString(),
                    subject = t.Title,
                    requester = t.Requester.FullName,
                    category = t.Category.CategoryName,
                    status = "Resolved"
                })
                .ToListAsync();
            return Ok(tickets);
        }

        // (GIỮ NGUYÊN) Hàm ClaimTicket (đã đúng logic)
        // Nó nhận 1 ticket "Wait" -> đổi thành "In Progress"
        [HttpPost("{id}/claim")]
        public async Task<IActionResult> ClaimTicket(int id, [FromBody] ClaimTicketDto dto)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null) return NotFound("Không tìm thấy ticket");

            // Chỉ cho phép claim nếu ticket đang "Wait"
            var currentStatus = await _context.TicketStatus.FindAsync(ticket.StatusId);
            if (currentStatus.StatusName != "Wait")
            {
                return BadRequest("Ticket này đã có KTV khác nhận hoặc đã đóng.");
            }

            var inProgressStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "In Progress");
            ticket.AssigneeId = dto.KtvId;
            ticket.StatusId = inProgressStatus.StatusId;
            ticket.UpdatedAt = DateTime.UtcNow;

            // Ghi lịch sử
            _context.TicketHistory.Add(new TicketHistory
            {
                TicketId = id,
                UserId = dto.KtvId,
                Action = "Change Status",
                OldValue = "Wait", // (THAY ĐỔI)
                NewValue = "In Progress",
                ChangedAt = DateTime.UtcNow
            });
            _context.TicketAssignment.Add(new TicketAssignment
            {
                TicketId = id,
                TechnicianId = dto.KtvId,
                AssignedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return Ok(new { message = "Nhận ticket thành công" });
        }

        // (GIỮ NGUYÊN) Hàm GetTicketById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = await _context.Ticket
                .Include(t => t.Requester).Include(t => t.Assignee).Include(t => t.Category).Include(t => t.Status)
                .Where(t => t.TicketId == id)
                .Select(t => new {
                    Title = t.Title,
                    Description = t.Description,
                    StatusName = t.Status.StatusName, // Trả về "Wait" hoặc "In Progress"
                    CategoryName = t.Category.CategoryName,
                    RequesterName = t.Requester.FullName,
                    AssigneeName = t.Assignee != null ? t.Assignee.FullName : "Đang chờ KTV", // (THAY ĐỔI)
                    IsAssigned = t.AssigneeId != null,
                    IsClosed = t.Status.StatusName == "Resolved"
                })
                .FirstOrDefaultAsync();
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        // (THAY ĐỔI) Hàm MarkTicketAsSolved
        [HttpPost("{id}/mark-solved")]
        public async Task<IActionResult> MarkTicketAsSolved(int id, [FromBody] MarkSolvedDto dto)
        {
            var ticket = await _context.Ticket.Include(t => t.Status).FirstOrDefaultAsync(t => t.TicketId == id);
            if (ticket == null) return NotFound();

            var resolvedStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "Resolved");
            if (resolvedStatus == null) return NotFound("Trạng thái 'Resolved' không tồn tại.");

            string oldStatusName = ticket.Status.StatusName; // Lấy tên status cũ

            ticket.StatusId = resolvedStatus.StatusId;
            ticket.UpdatedAt = DateTime.UtcNow;

            // Nếu ticket là "New" (tự giải quyết), gán cho SV (hoặc Admin)
            // Nếu ticket là "Wait" hoặc "In Progress", nó đã có (hoặc sẽ có) KTV
            if (ticket.AssigneeId == null)
            {
                ticket.AssigneeId = dto.StudentId; // Gán cho chính SV
            }

            _context.TicketHistory.Add(new TicketHistory
            {
                TicketId = id,
                UserId = dto.StudentId,
                Action = "Change Status",
                OldValue = oldStatusName,
                NewValue = "Resolved",
                ChangedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return Ok(new { message = "Ticket đã được đóng." });
        }

        // (GIỮ NGUYÊN) Hàm GetMyRequests
        [HttpGet("my-requests/{studentId}")]
        public async Task<IActionResult> GetMyRequests(int studentId, [FromQuery] string? searchTitle, [FromQuery] int? categoryId, [FromQuery] int? statusId, [FromQuery] int page = 1)
        { /* ... (Code giữ nguyên) ... */
            const int pageSize = 10;
            var query = _context.Ticket.Include(t => t.Category)
                .Include(t => t.Status)
                .Where(t => t.UserId == studentId && t.IsDeleted == false);
            if (!string.IsNullOrEmpty(searchTitle)) { query = query
                    .Where(t => t.Title.ToLower()
                    .Contains(searchTitle.ToLower())); }
            if (categoryId.HasValue && categoryId > 0) { query = query.Where(t => t.CategoryId == categoryId); }
            if (statusId.HasValue && statusId > 0) { query = query.Where(t => t.StatusId == statusId); }
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var myTickets = await query.OrderByDescending(t => t.UpdatedAt).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(t => new { Id = t.TicketId, Title = t.Title, CategoryName = t.Category.CategoryName, StatusName = t.Status.StatusName })
                .ToListAsync();
            return Ok(new { Tickets = myTickets, TotalPages = totalPages, CurrentPage = page });
        }
    }
}