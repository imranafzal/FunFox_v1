using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace funfox_App.Models
{
    public class Class
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public int? GradeLevel { get; set; }
        public string Timings { get; set; }
        public int? MaxClassSize { get; set; }
        public int ProgramID { get; set; }
    }
}
