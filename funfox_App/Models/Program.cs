using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace funfox_App.Models
{
    public class Program
    {
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
