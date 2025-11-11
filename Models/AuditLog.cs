// Models/AuditLog.cs
// (Lưu ý: Bảng này không có IsDeleted/RowVersion, đúng theo thiết kế)
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSupportBE.Api.Models
{
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required]
        public int UserId { get; set; } // Người thực hiện

        [Required]
        [MaxLength(100)]
        public string Action { get; set; } // "Login"

        [Required]
        [MaxLength(100)]
        public string Entity { get; set; } // "User"

        public int? EntityId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}