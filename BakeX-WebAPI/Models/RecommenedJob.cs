namespace BakeX_WebAPI.Models
{
    using System;

    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string JobDescription { get; set; }
        public string JobType { get; set; }

        public int PostedById { get; set; }
        public DateTime CreatedDate { get; set; }
      
        public string DistrictName { get; set; }
        public string ExperienceType { get; set; }

        public int AppliedStatus { get; set; }
    }

}
