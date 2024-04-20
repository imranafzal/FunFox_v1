using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Program
    {
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
