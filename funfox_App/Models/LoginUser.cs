using System.ComponentModel.DataAnnotations;

namespace funfox_App.Models
{
    public class LoginUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
