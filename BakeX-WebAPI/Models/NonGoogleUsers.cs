using System.ComponentModel.DataAnnotations;

namespace BakeX_WebAPI.Models
{
    public class NonGoogleUser
    { 
        [Required]
        public string PhoneNum { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
