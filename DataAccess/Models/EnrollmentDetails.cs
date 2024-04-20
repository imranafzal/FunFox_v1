using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class EnrollmentDetails
    {
        public int EnrollmentID { get; set; }
        public int? UserID { get; set; }
        public int? ClassID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassName { get; set; }
        public int? GradeLevel { get; set; }
        public string Timings { get; set; }
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }

    }
}
