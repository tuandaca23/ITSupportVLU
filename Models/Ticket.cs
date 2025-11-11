// Models/Ticket.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSupportBE.Api.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }

        [Required]
        public int UserId { get; set; } // Người tạo (Student)

        public int? AssigneeId { get; set; } // KTV xử lý

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // --- THUỘC TÍNH ĐIỀU HƯỚNG (NAVIGATION PROPERTIES) ---
        // (Đây KHÔNG phải là cột trong CSDL. 
        //  Chúng dùng để viết code LINQ (truy vấn) đơn giản và hiệu quả hơn,
        //  thay vì phải viết JOIN thủ công.)
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User Requester { get; set; } // Đổi tên từ 'User' thành 'Requester'

        [ForeignKey("AssigneeId")]
        public virtual User Assignee { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("StatusId")]
        public virtual TicketStatus Status { get; set; }
    }
}