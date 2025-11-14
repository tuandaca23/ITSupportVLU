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
        private readonly ITSupportDbContext _context; // (GHI NHỚ) Dùng DbContext của anh

        public TicketsController(ITSupportDbContext context)
        {
            _context = context;
        }

        // Lấy yêu cầu của sinh viên
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            // ... (Logic CreateTicket, Check KB, Auto-reply) ...

            // (Lấy code đầy đủ từ câu trả lời trước)
            var newStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "New");
            if (newStatus == null) return NotFound("Trạng thái 'New' không tồn tại.");
            // ---(GHI CHÚ) ĐÂY LÀ PHẦN TẠO TICKET ĐẦY ĐỦ(KHÔNG TÓM TẮT)-- -
            var newTicket = new Ticket
            {
                UserId = dto.StudentId,       // Lấy từ DTO (Frontend)
                Title = dto.Title,          // Lấy từ DTO
                Description = dto.Description,  // Lấy từ DTO
                CategoryId = dto.CategoryId,  // Lấy từ DTO
                StatusId = newStatus.StatusId, // Gán StatusId = 1 (New)
                AssigneeId = null,          // Mới tạo, chưa gán KTV
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
                // (ResolvedAt là NULL theo mặc định)
            };
            _context.Ticket.Add(newTicket);
            await _context.SaveChangesAsync();

            // Tìm kiếm bài viết KB phù hợp
            string searchQuery = dto.Title + " " + dto.Description;
            var keywords = searchQuery.ToLower()
                .Split(new[] { ' ', ',', '.', '?', '!' },
                StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .Where(k => k.Length > 2)
                .ToList();
            var allArticles = await _context.KBArticle.
                Where(a => a.IsActive && !a.IsDeleted).
                ToListAsync();
            var bestMatch = allArticles
                .Select(article => new
                {
                    Article = article,
                    MatchCount = keywords.Count(k => article.Title.ToLower().Contains(k))
                })
                .Where(x => x.MatchCount > 0)
                .OrderByDescending(x => x.MatchCount)
                .FirstOrDefault();
            var adminUser = await _context.User
                .FirstOrDefaultAsync(u => u.Role.RoleName == "Admin");

            // Tự động phản hồi nếu tìm thấy bài viết phù hợp
            if (bestMatch != null && bestMatch.MatchCount > 1)
            {
                string huongDanPlainText = bestMatch.Article.Content;
                var autoReply = new TicketReply
                {
                    TicketId = newTicket.TicketId,
                    UserId = adminUser.UserId,
                    Message = $"[TỰ ĐỘNG] Cảm ơn bạn đã gửi yêu cầu.\n" +
                              $"Hệ thống tìm thấy một bài viết có thể liên quan:\n\n" +
                              $"---[ HƯỚNG DẪN: {bestMatch.Article.Title} ]---\n\n" +
                              $"{huongDanPlainText}\n\n" +
                              $"--------------------------------------------------\n" +
                              $"Để xem bài viết gốc (có hình ảnh), vui lòng truy cập:\n" +
                              $"{bestMatch.Article.OriginalURL}\n\n" +
                              $"(Nếu bài viết này không giải quyết được, KTV sẽ sớm kết nối với bạn.)",
                    CreatedAt = DateTime.UtcNow
                };
                _context.TicketReply.Add(autoReply);
                _context.Notification.Add(new Notification { UserId = dto.StudentId, TicketId = newTicket.TicketId, Message = "Bạn có một phản hồi tự động cho ticket #" + newTicket.TicketId });
            }
            else
            {
                // Gửi thông báo "chuông" cho TẤT CẢ KTV nếu không tìm thấy bài viết phù hợp
                var ktvs = await _context.User.Where(u => u.Role.RoleName == "KTV" && u.IsActive).ToListAsync();
                foreach (var ktv in ktvs)
                {
                    _context.Notification.Add(new Notification { UserId = ktv.UserId, TicketId = newTicket.TicketId, Message = $"Ticket mới #{newTicket.TicketId} ({dto.Title}) vừa được tạo." });
                }
            }
            await _context.SaveChangesAsync();
            return Ok(newTicket);
        }

        // Nâng cấp GetTicketById để trả về thêm thông tin
        // Mục đích: Cho RequestDetail.vue biết khi nào cần ẩn/hiện nút
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = await _context.Ticket
                .Include(t => t.Requester)
                .Include(t => t.Assignee)
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Where(t => t.TicketId == id)
                .Select(t => new
                {
                    Title = t.Title,
                    Description = t.Description,
                    StatusName = t.Status.StatusName,
                    CategoryName = t.Category.CategoryName,
                    RequesterName = t.Requester.FullName,
                    AssigneeName = t.Assignee != null ? t.Assignee.FullName : "Chưa ai nhận",

                    // Cờ (flag) quan trọng cho logic Frontend
                    IsAssigned = t.AssigneeId != null,
                    IsClosed = t.Status.StatusName == "Resolved" // (GHI NHỚ) Giả sử "Resolved" là tên trạng thái đóng
                })
                .FirstOrDefaultAsync();

            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        // API cho nút "Đã giải quyết"
        // POST: api/tickets/{id}/mark-solved
        [HttpPost("{id}/mark-solved")]
        public async Task<IActionResult> MarkTicketAsSolved(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null) return NotFound();

            // (GHI NHỚ) Đảm bảo có Status "Resolved" (StatusId=3) trong CSDL
            var resolvedStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "Resolved");
            if (resolvedStatus == null) return NotFound("Trạng thái 'Resolved' không tồn tại.");

            ticket.StatusId = resolvedStatus.StatusId;
            ticket.UpdatedAt = DateTime.UtcNow;

            // (GHI NHỚ) Gán Admin (UserId=3) làm người "giải quyết" cho ticket tự động
            ticket.AssigneeId = 3;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Ticket đã được đóng." });
        }

        // API cho nút "Kết nối KTV"
        // POST: api/tickets/{id}/request-ktv
        [HttpPost("{id}/request-ktv")]
        public async Task<IActionResult> RequestKtv(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null) return NotFound();

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

        // Nâng cấp GetMyRequests để hỗ trợ Tìm kiếm, Lọc, Phân trang
        // GET: api/tickets/my-requests/{studentId}?searchTitle=...&categoryId=...&statusId=...&page=1
        [HttpGet("my-requests/{studentId}")]
        public async Task<IActionResult> GetMyRequests(
            int studentId,
            [FromQuery] string? searchTitle,
            [FromQuery] int? categoryId,
            [FromQuery] int? statusId,
            [FromQuery] int page = 1)
        {
            const int pageSize = 10; // (GHI NHỚ) 10 yêu cầu/trang

            var query = _context.Ticket
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Where(t => t.UserId == studentId && t.IsDeleted == false);

            // 1. Lọc theo Tiêu đề (Search)
            if (!string.IsNullOrEmpty(searchTitle))
            {
                query = query.Where(t => t.Title.ToLower().Contains(searchTitle.ToLower()));
            }

            // 2. Lọc theo Phân loại (Dropdown 1)
            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(t => t.CategoryId == categoryId);
            }

            // 3. Lọc theo Trạng thái (Dropdown 2)
            if (statusId.HasValue && statusId > 0)
            {
                query = query.Where(t => t.StatusId == statusId);
            }

            // Lấy tổng số lượng (để tính số trang)
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // 4. Phân trang (Pagination)
            var myTickets = await query
                .OrderByDescending(t => t.UpdatedAt)
                .Skip((page - 1) * pageSize) // Bỏ qua các trang trước
                .Take(pageSize) // Lấy 10
                .Select(t => new
                {
                    Id = t.TicketId,
                    Title = t.Title,
                    CategoryName = t.Category.CategoryName,
                    StatusName = t.Status.StatusName
                })
                .ToListAsync();

            // Trả về một object phức tạp chứa cả dữ liệu và thông tin trang
            return Ok(new
            {
                Tickets = myTickets,
                TotalPages = totalPages,
                CurrentPage = page
            });
        }

        // Hàm GetNewTickets
        [HttpGet("queue/new")]
        public async Task<IActionResult> GetNewTickets()
        {
            // ... (Code cũ giữ nguyên) ...
            var tickets = await _context.Ticket
                .Include(t => t.Requester).Include(t => t.Category)
                .Where(t => t.Status.StatusName == "New" && t.IsDeleted == false)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new
                {
                    id = t.TicketId.ToString(),
                    subject = t.Title,
                    requester = t.Requester.FullName,
                    category = t.Category.CategoryName,
                    status = "New",
                    updatedAt = t.UpdatedAt.ToString("dd-MM-yyyy")
                }).ToListAsync();
            return Ok(tickets);
        }

        // Hàm ClaimTicket
        [HttpPost("{id}/claim")]
        public async Task<IActionResult> ClaimTicket(int id, [FromBody] ClaimTicketDto dto)
        {
            // ... (Code cũ giữ nguyên) ...
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null) return NotFound("Không tìm thấy ticket");
            var inProgressStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "In Progress");
            ticket.AssigneeId = dto.KtvId; ticket.StatusId = inProgressStatus.StatusId; ticket.UpdatedAt = DateTime.UtcNow;
            _context.TicketHistory.Add(new TicketHistory { TicketId = id, UserId = dto.KtvId, Action = "Change Status", OldValue = "New", NewValue = "In Progress", ChangedAt = DateTime.UtcNow });
            _context.TicketAssignment.Add(new TicketAssignment { TicketId = id, TechnicianId = dto.KtvId, AssignedAt = DateTime.UtcNow });
            await _context.SaveChangesAsync();
            return Ok(new { message = "Nhận ticket thành công" });
        }
    }
}