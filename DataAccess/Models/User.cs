using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{

    public class User
    {
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
