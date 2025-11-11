namespace ITSupportBE.Api.DTOs
{
    // DTO này khớp với payload mà NewTickets.vue sẽ gửi
    public class ClaimTicketDto
    {
        public int KtvId { get; set; } // Sẽ lấy từ localStorage
    }
}