using ITSupportBE.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ITSupportBE.Api.Controllers
{
    // (GHI NHỚ) đặt route là "statuses" cho ngắn gọn
    [Route("api/statuses")]
    [ApiController]
    public class TicketStatusController : ControllerBase
    {
        // (GHI NHỚ) Sử dụng đúng tên DbContext 
        private readonly ITSupportDbContext _context;

        public TicketStatusController(ITSupportDbContext context)
        {
            _context = context;
        }

        // GET: api/statuses
        // API này sẽ cung cấp dữ liệu cho dropdown "Lọc theo Trạng thái"
        [HttpGet]
        public async Task<IActionResult> GetStatuses()
        {
            var statuses = await _context.TicketStatus
                .Where(s => s.IsDeleted == false)
                .Select(s => new { s.StatusId, s.StatusName })
                .ToListAsync();

            return Ok(statuses);
        }
    }
}