namespace BakeX_WebAPI.Models
{
    public class JobPost
    {
        public int? Id { get; set; }
        public int PostedById { get; set; }

        public string Title { get; set; }
        public int JobTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ExperienceId { get; set; }
        public int BusinessId { get; set; }
        public string JobDescription { get; set; }
        public string Salary { get; set; }
        public int DistrictId { get; set; }
        public char? IsActive { get; set; }

        public byte[]? ProfileImage { get; set; }

        public List<int> ExpertiseIds { get; set; }
    }
}
