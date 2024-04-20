using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace funfox_App.Models
{

    public class User
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "Please enter a value.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a value.")]
        public string LastName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please enter a value.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a value.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter a value.")]
        public string Username { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? CreatedAt { get; set; }
    }
}
