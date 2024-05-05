namespace BakeX_WebAPI.Models
{
    public class User
    {
        public int? Id { get; set; }
        public required string? MobileNumber { get; set; }

        public required int? UserTypeId { get; set; }
        public required string? IsMobileVerified { get; set; }

        public string ? GoogleId { get; set; }

        public String ? Password { get; set; }

        public required int AuthId { get; set; }
        public  DateTime? CreatedAt { get; set; }
    }
}
