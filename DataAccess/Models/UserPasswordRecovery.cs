using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class UserPasswordRecovery
    {
        public string RecoveryToken { get; set; }
        public DateTime? RecoveryTokenExpiry { get; set; }
        public string LoginName { get; set; }
    }
}
