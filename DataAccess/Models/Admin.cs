using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Admin
    {
        public int AdminID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
