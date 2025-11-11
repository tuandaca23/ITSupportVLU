using ITSupportBE.Api.Data;
using ITSupportBE.Api.DTOs;
using ITSupportBE.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITSupportBE.Api.Controllers
{
    // Chúng ta đặt API này lồng vào "tickets" cho đúng chuẩn RESTful
    [Route("api/tickets")]
    [ApiController]
    public class TicketReplyController : ControllerBase
    {
        private readonly ITSupportDbContext _context;

        public TicketReplyController(ITSupportDbContext context)
        {
            _context = context;
        }

        // GET: api/tickets/{ticketId}/replies
        // Lấy lịch sử chat cho 1 ticket
        [HttpGet("{ticketId}/replies")]
        public async Task<IActionResult> GetReplies(int ticketId)
        {
            var replies = await _context.TicketReply
                .Include(r => r.User) // Join User để lấy tên người gửi
                .Where(r => r.TicketId == ticketId && r.IsDeleted == false)
                .OrderBy(r => r.CreatedAt)
                .Select(r => new {
                    SenderName = r.User.FullName,
                    Message = r.Message,
                    Timestamp = r.CreatedAt,
                    IsKtvMessage = r.User.Role.RoleName != "Student" // Giúp Vue biết tin nhắn của KTV hay SV
                })
                .ToListAsync();

            return Ok(replies);
        }

        // POST: api/tickets/{ticketId}/replies
        // SV hoặc KTV gửi 1 tin nhắn
        [HttpPost("{ticketId}/replies")]
        public async Task<IActionResult> PostReply(int ticketId, [FromBody] CreateReplyDto dto)
        {
            var newReply = new TicketReply
            {
                TicketId = ticketId,
                UserId = dto.UserId, // Lấy UserId (SV hoặc KTV) từ DTO
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow
            };
            _context.TicketReply.Add(newReply);

            // Logic Thông báo (cho người còn lại)
            var ticket = await _context.Ticket.FindAsync(ticketId);
            if (ticket == null) return NotFound("Ticket không tồn tại");

            // Xác định người nhận
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