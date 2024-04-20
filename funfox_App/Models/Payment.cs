using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace funfox_App.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int? UserID { get; set; }
        public int? ClassID { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
