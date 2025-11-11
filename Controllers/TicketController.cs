using ITSupportBE.Api.Data;     //  namespace của DbContext
using ITSupportBE.Api.DTOs;      // Using thư mục DTOs
using ITSupportBE.Api.Models;  // namespace của Models
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITSupportBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITSupportDbContext _context;

        public TicketsController(ITSupportDbContext context)
        {
            _context = context;
        }

        // POST: api/tickets
        // === YÊU CẦU CỐT LÕI (PHẦN 1): SINH VIÊN GỬI TICKET ===
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
        {
            // 1. Lấy trạng thái "New" (StatusId = 1)
            var newStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "New");
            if (newStatus == null) return NotFound("Trạng thái 'New' không tồn tại.");

            // 2. Tạo Ticket mới
            var newTicket = new Ticket
            {
                UserId = dto.StudentId, // Lấy StudentId từ DTO (Không cần Auth)
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                StatusId = newStatus.StatusId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _context.Ticket.Add(newTicket);

            // Phải Save 1 lần để lấy được newTicket.TicketId
            await _context.SaveChangesAsync();

            // === YÊU CẦU CỐT LÕI (PHẦN 2): TỰ ĐỘNG PHẢN HỒI ===
            var kbArticle = await _context.KBArticle
                .FirstOrDefaultAsync(k => k.Title.Contains(dto.Title) && !k.IsDeleted);

            var adminUser = await _context.User.FirstOrDefaultAsync(u => u.Role.RoleName == "Admin");

            if (kbArticle != null)
            {
                // 3a. NẾU CÓ KB: Tự động trả lời
                var autoReply = new TicketReply
                {
                    TicketId = newTicket.TicketId,
                    UserId = adminUser.UserId, // Gửi với tư cách Admin (UserId=3)
                    Message = $"[TỰ ĐỘNG] Chúng tôi tìm thấy một bài viết có thể giải quyết vấn đề của bạn: '{kbArticle.Title}'. Vui lòng xem thử trước khi KTV kết nối.",
                    CreatedAt = DateTime.UtcNow
                };
                _context.TicketReply.Add(autoReply);

                // Tạo thông báo cho SV
                _context.Notification.Add(new Notification
                {
                    UserId = dto.StudentId,
                    TicketId = newTicket.TicketId,
                    Message = "Bạn có một phản hồi tự động cho ticket #" + newTicket.TicketId
                });
            }
            else
            {
                // 3b. NẾU KHÔNG CÓ KB: Thông báo cho KTV (Phần 3 của Demo)
                var ktvs = await _context.User.Where(u => u.Role.RoleName == "KTV" && u.IsActive).ToListAsync();
                foreach (var ktv in ktvs)
                {
                    _context.Notification.Add(new Notification
                    {
                        UserId = ktv.UserId,
                        TicketId = newTicket.TicketId,
                        Message = $"Ticket mới #{newTicket.TicketId} ({dto.Title}) vừa được tạo."
                    });
                }
            }

            // Lưu lần 2 (lưu Reply/Notification)
            await _context.SaveChangesAsync();
            return Ok(newTicket);
        }

        // GET: api/tickets/queue/new
        // Lấy danh sách ticket Mới cho KTV Dashboard (NewTickets.vue)
        [HttpGet("queue/new")]
        public async Task<IActionResult> GetNewTickets()
        {
            var tickets = await _context.Ticket
                .Include(t => t.Requester) // Join User (để lấy FullName)
                .Include(t => t.Category)  // Join Category (để lấy CategoryName)
                .Where(t => t.Status.StatusName == "New" && t.IsDeleted == false)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new {
                    // Trả về JSON khớp với file NewTickets.vue
                    id = t.TicketId.ToString(), // Vue dùng "id" (string)
                    subject = t.Title,
                    requester = t.Requester.FullName,
                    category = t.Category.CategoryName,
                    status = "New",
                    updatedAt = t.UpdatedAt.ToString("dd-MM-yyyy")
                })
                .ToListAsync();
            return Ok(tickets);
        }

        // POST: api/tickets/{id}/claim
        // KTV "Nhận" ticket từ NewTickets.vue
        [HttpPost("{id}/claim")]
        public async Task<IActionResult> ClaimTicket(int id, [FromBody] ClaimTicketDto dto)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null) return NotFound("Không tìm thấy ticket");

            var inProgressStatus = await _context.TicketStatus.FirstOrDefaultAsync(s => s.StatusName == "In Progress");

            // 1. Cập nhật Ticket
            ticket.AssigneeId = dto.KtvId; // Lấy KtvId từ DTO
            ticket.StatusId = inProgressStatus.StatusId;
            ticket.UpdatedAt = DateTime.UtcNow;

            // 2. Ghi Lịch sử (Theo ERD của anh)
            _context.TicketHistory.Add(new TicketHistory
            {
                TicketId = id,
                UserId = dto.KtvId,
                Action = "Change Status",
                OldValue = "New",
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
    }
}