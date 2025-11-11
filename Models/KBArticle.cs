using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSupportBE.Api.Models
{
    
    public class KBArticle
    {
        [Key] // PK (Khóa chính)
        public int ArticleId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } // Tiêu đề bài viết

        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Content { get; set; } // Nội dung bài viết

        [Required]
        [StringLength(500)]
        public string OriginalURL { get; set; } // Link gốc (UNIQUE)

        [StringLength(100)]
        public string? Category { get; set; } // Lấy từ web gốc

        [StringLength(500)]
        public string? Tags { get; set; } // Các từ khóa (sẽ NULL khi import)

        public bool IsActive { get; set; } = true; // Cho phép bật/tắt

        public DateTime LastCrawledAt { get; set; } = DateTime.UtcNow; // Thời điểm nhập dữ liệu

        public bool IsDeleted { get; set; } = false; // Cờ xóa mềm

        [Timestamp] // Tự động xử lý RowVersion
        public byte[] RowVersion { get; set; }
    }
}