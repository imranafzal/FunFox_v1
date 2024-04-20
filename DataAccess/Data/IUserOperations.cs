using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public interface IUserOperations
    {
        Task<string> Add(User model);
        Task<string> UpdateProfile(User model);

        Task<User> FindById(int UserId);

        Task<User> FindByLoginName(string loginname);

        Task<string> Delete(int userID);
        Task<List<User>> List();

    }
}
