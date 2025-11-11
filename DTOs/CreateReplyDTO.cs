namespace ITSupportBE.Api.DTOs
{
    // DTO này dùng cho cả KTVRequestDetail.vue và RequestDetail.vue
    public class CreateReplyDto
    {
        public int UserId { get; set; } // Người gửi (SV hoặc KTV)
        public string Message { get; set; }
    }
}