namespace BakeX_WebAPI.Models
{
    public class Profile
    {

            public int? ProfileId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Age { get; set; }
            public string MobileNo { get; set; }
            public string Gender { get; set; }
            public string State { get; set; }
           
            public string District { get; set; } 
            public string Place { get; set; } 
            public string ProfileCreatedDate { get; set; }
            public string Pincode { get; set; }
            public int EducationId { get; set; }

            public int ExperienceId { get; set; } 

            public List<int> ExpertiseIds { get; set; }

            public List<int> JobTypeIds { get; set; }



    }
}
