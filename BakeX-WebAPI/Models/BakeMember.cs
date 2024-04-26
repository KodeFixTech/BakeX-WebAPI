namespace BakeX_WebAPI.Models
{
    public class BakeMember
    {
        public int MemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNo { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Place { get; set; }
        public string Pincode { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string FssaiNo { get; set; }
        public string BusinessPhone { get; set; }
        public DateTime? FssaiExpiryDate { get; set; }
        public DateTime? ProfileCreatedDate { get; set; }
        public string? LogoUrl { get; set; }

        public string? IsBakeKeralaMember { get; set; }
    }

}
