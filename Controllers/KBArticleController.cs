using ITSupportBE.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITSupportBE.Api.Controllers
{
    [Route("api/kb")]
    [ApiController]
    public class KBArticleController : ControllerBase
    {
        private readonly ITSupportDbContext _context;

        public KBArticleController(ITSupportDbContext context)
        {
            _context = context;
        }

        // GET: api/kb/popular
        // (GIỮ NGUYÊN) API LẤY BÀI VIẾT PHỔ BIẾN
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularArticles()
        {
            var articles = await _context.KBArticle
                .Where(a => a.IsActive && !a.IsDeleted)
                .OrderByDescending(a => a.LastCrawledAt)
                .Take(3)
                .Select(a => new { a.ArticleId, a.Title, a.OriginalURL }) // Sửa: Thêm OriginalURL
                .ToListAsync();
            return Ok(articles);
        }

        // GET: api/kb/search?query=...
        // === (THAY ĐỔI) API TÌM KIẾM ĐÃ NÂNG CẤP ===
        [HttpGet("search")]
        public async Task<IActionResult> SearchArticles([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // Nếu query rỗng, trả về 3 bài phổ biến
                return await GetPopularArticles();
            }

            // 1. Tách chuỗi query thành mảng các từ khóa
            // Ví dụ: "Hướng dẫn VLID" -> ["hướng", "dẫn", "vlid"]
            var keywords = query.ToLower()
                                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // 2. Xây dựng truy vấn (query) động
            var queryable = _context.KBArticle
                                    .Where(a => a.IsActive && !a.IsDeleted);

            // 3. Lặp qua từng từ khóa và áp dụng điều kiện .Contains()
            // Logic: Title PHẢI chứa TẤT CẢ các từ khóa
            foreach (var keyword in keywords)
            {
                queryable = queryable.Where(a => a.Title.ToLower().Contains(keyword));
                // || a.Content.ToLower().Contains(keyword)); 
                // (Bỏ a.Content để tìm kiếm nhanh hơn,
                //  anh có thể mở lại nếu muốn tìm cả nội dung)
            }

            // 4. Lấy kết quả
            var articles = await queryable
                .Take(5) // Giới hạn 5 kết quả
                .Select(a => new { a.ArticleId, a.Title, a.OriginalURL }) // Sửa: Thêm OriginalURL
                .ToListAsync();

            return Ok(articles);
        }
    }
}