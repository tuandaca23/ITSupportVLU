using ITSupportBE.Api.Data; // Thay bằng namespace của DbContext
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITSupportBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ITSupportDbContext _context;

        public CategoriesController(ITSupportDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        // Lấy danh sách phân loại để hiển thị trên form
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Category // Dùng tên số ít
                .Where(c => c.IsDeleted == false)
                .Select(c => new { c.CategoryId, c.CategoryName })
                .ToListAsync();

            return Ok(categories);
        }
    }
}