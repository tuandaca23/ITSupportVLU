using ITSupportBE.Api.Data;
using ITSupportBE.Api.DTOs;
using ITSupportBE.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ITSupportBE.Api.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketReplyController : ControllerBase
    {
        // (GHI CHÚ: GIỮ NGUYÊN) Sử dụng DbContext của anh
        private readonly ITSupportDbContext _context;

        public TicketReplyController(ITSupportDbContext context)
        {
            _context = context;
        }

        // GET: api/tickets/{ticketId}/replies
        [HttpGet("{ticketId}/replies")]
        public async Task<IActionResult> GetReplies(int ticketId)
        {
            var replies = await _context.TicketReply
                .Include(r => r.User)
                .Include(r => r.User.Role) // Join Role
                .Where(r => r.TicketId == ticketId && r.IsDeleted == false)
                .OrderBy(r => r.CreatedAt)
                .Select(r => new {

                    // (GHI CHÚ: GIỮ NGUYÊN) Logic "Hệ thống" (anh yêu cầu)
                    SenderName = (r.User.Role.RoleName == "Admin" && r.Message.StartsWith("[TỰ ĐỘNG]"))
                                    ? "Hệ thống"
                                    : r.User.FullName,

                    Message = r.Message,
                    Timestamp = r.CreatedAt,

                    // === (GHI CHÚ: THAY ĐỔI LỚN) ===
                    // Xóa "IsKtvMessage"
                    // Trả về vai trò thật của người gửi
                    SenderRole = r.User.Role.RoleName // Sẽ trả về "Student", "KTV", "Admin"
                })
                .ToListAsync();

            return Ok(replies);
        }

        // POST: api/tickets/{ticketId}/replies
        // (GHI CHÚ: GIỮ NGUYÊN) Hàm này không cần sửa
        [HttpPost("{ticketId}/replies")]
        public async Task<IActionResult> PostReply(int ticketId, [FromBody] CreateReplyDto dto)
        {
            var newReply = new TicketReply
            {
                TicketId = ticketId,
                UserId = dto.UserId,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow
            };
            _context.TicketReply.Add(newReply);

            var ticket = await _context.Ticket.FindAsync(ticketId);
            if (ticket == null) return NotFound("Ticket không tồn tại");

            int receiverId = (dto.UserId == ticket.UserId) ? ticket.AssigneeId.GetValueOrDefault() : ticket.UserId;

            if (receiverId > 0)
            {
                var sender = await _context.User.FindAsync(dto.UserId);
                _context.Notification.Add(new Notification
                {
                    UserId = receiverId,
                    TicketId = ticketId,
                    Message = $"{sender.FullName} đã phản hồi ticket #{ticketId}."
                });
            }

            await _context.SaveChangesAsync();
            return Ok(newReply);
        }
    }
}