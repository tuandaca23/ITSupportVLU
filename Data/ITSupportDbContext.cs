using Microsoft.EntityFrameworkCore;
using ITSupportBE.Api.Models;

namespace ITSupportBE.Api.Data
{
    public class ITSupportDbContext : DbContext
    {
        public ITSupportDbContext(DbContextOptions<ITSupportDbContext> options)
            : base(options)
        {
        }

        // BẢNG CẤU HÌNH
        public DbSet<Role> Role { get; set; }
        public DbSet<Category> Categorie { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }

        // BẢNG NGƯỜI DÙNG
        public DbSet<User> User { get; set; }

        // BẢNG NGHIỆP VỤ (CORE)
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketReply> TicketReplie { get; set; }
        public DbSet<InternalNote> InternalNote { get; set; } // Bảng mới
        public DbSet<TicketAttachment> TicketAttachment { get; set; }

        // BẢNG HỖ TRỢ & LỊCH SỬ
        public DbSet<TicketAssignment> TicketAssignment { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<KBArticle> KBArticle { get; set; }
        public DbSet<CannedResponse> CannedResponse { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----------------- Cấu hình quan hệ -----------------
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Requester)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketReply>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InternalNote>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketAssignment>()
                .HasOne(a => a.Technician)
                .WithMany()
                .HasForeignKey(a => a.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketHistory>()
                .HasOne(h => h.User)
                .WithMany()
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----------------- Global Query Filter (Soft Delete) -----------------
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Ticket>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<TicketReply>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<InternalNote>().HasQueryFilter(n => !n.IsDeleted);
            modelBuilder.Entity<TicketAttachment>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<TicketAssignment>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<TicketHistory>().HasQueryFilter(h => !h.IsDeleted);
            modelBuilder.Entity<Notification>().HasQueryFilter(n => !n.IsDeleted);
            modelBuilder.Entity<CannedResponse>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<KBArticle>().HasQueryFilter(k => !k.IsDeleted);

            // ----------------- Các mối quan hệ khác nếu cần -----------------
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CannedResponse>()
                .HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<InternalNote>()
                .HasOne(n => n.Ticket)
                .WithMany()
                .HasForeignKey(n => n.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketAttachment>()
                .HasOne(a => a.Ticket)
                .WithMany()
                .HasForeignKey(a => a.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketAttachment>()
                .HasOne(a => a.TicketReply)
                .WithMany()
                .HasForeignKey(a => a.ReplyId);
        }
    }
}
