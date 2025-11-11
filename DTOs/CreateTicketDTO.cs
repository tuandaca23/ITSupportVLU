namespace ITSupportBE.Api.DTOs
{
    // DTO này khớp với payload mà StudentDashboard.vue sẽ gửi
    public class CreateTicketDto
    {
        public int StudentId { get; set; } // Sẽ lấy từ localStorage
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}