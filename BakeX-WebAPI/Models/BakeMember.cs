namespace BakeX_WebAPI.Models
{
    public class BakeMember
    {
        public int MemberId { get; set; }

        public string? MemberName { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNo { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Place { get; set; }
        public string PinCode { get; set; }

        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string FssaiLicenseNo { get; set; }
        public string BusinessPhone { get; set; }
        public DateTime? FssaiExpiryDate { get; set; }
        public DateTime? ProfileCreatedDate { get; set; }


        public byte[]? ProfileImage { get; set; }

        public string? IsBakeKeralaMember { get; set; }

        public string? ProfileImageBase64 { get; set; }
    }

}
