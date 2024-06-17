namespace BakeX_WebAPI.Models
{
    public class JobSeeker
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StateName { get; set; }

        public string Gender { get; set; }
        public string Place {  get; set; }


        public string DistrictName { get; set; }
        public string ExperienceType { get; set; }
        public string EducationLevel { get; set; }
        public string PinCode { get; set; }

        public int Age { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
