namespace BakeX_WebAPI.Models
{
    public class BakeMember
    {
        public int MembershipId { get; set; }
        public string MemberName { get; set; }
        public string Phone { get; set; }
        public string BusinessName { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessAddress { get; set; }
        public string PinCode { get; set; }
        public string FSSAILicenseNo { get; set; }
        public int DistrictId { get; set; }
        public int MandalamId { get; set; }
        public string Designation { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? MembershipExpiry { get; set; }
        public DateTime? Age { get; set; }
    }
}
