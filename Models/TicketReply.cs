// Models/TicketReply.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSupportBE.Api.Models
{
    public class TicketReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplyId { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required]
        public int UserId { get; set; } // Người gửi (SV hoặc KTV)

        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // --- THUỘC TÍNH ĐIỀU HƯỚNG (NAVIGATION PROPERTIES) ---
        // (Đây KHÔNG phải là cột trong CSDL. 
        //  Chúng dùng để viết code LINQ (truy vấn) đơn giản và hiệu quả hơn,
        //  thay vì phải viết JOIN thủ công.)
        // Navigation properties
        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}