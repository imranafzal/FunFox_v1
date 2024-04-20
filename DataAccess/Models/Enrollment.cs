using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int? UserID { get; set; }
        public int? ClassID { get; set; }
        public DateTime? EnrollmentDate { get; set; }
    }
}
