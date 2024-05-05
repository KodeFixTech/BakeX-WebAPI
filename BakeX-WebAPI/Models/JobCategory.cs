namespace BakeX_WebAPI.Models
{
    public class JobCategory
    {
       
            public int Id { get; set; }
            public string job_category_name { get; set; }
        
    }

    public class EmploymentInformation
    {
        public int JobTypeId { get; set; }
        public string JobType { get; set; }
    }
}
