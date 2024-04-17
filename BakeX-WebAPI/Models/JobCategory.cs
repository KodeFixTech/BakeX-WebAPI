namespace BakeX_WebAPI.Models
{
    public class JobCategory
    {
       
            public int Id { get; set; }
            public string job_category_name { get; set; }
        
    }

    public class EmploymentInformation
    {
        public int EmploymentId { get; set; }
        public string EmploymentType { get; set; }
    }
}
