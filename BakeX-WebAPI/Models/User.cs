namespace BakeX_WebAPI.Models
{
    public class User
    {
        public int User_Id { get; set; }
        public required string email { get; set; }

        public required string googleId { get; set; }
        public required string name { get; set; }
        public required string profileImage { get; set; }
    }
}
