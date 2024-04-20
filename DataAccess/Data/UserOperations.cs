using DataAccess.DbAccess;
using DataAccess.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class UserOperations : IUserOperations
    {
        private readonly ISqlDataAccess _dbo;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private ILogger<UserOperations> _logger;

        public UserOperations(ISqlDataAccess dbo, IConfiguration config, IMemoryCache cache, ILogger<UserOperations> logger)
        {
            _dbo = dbo;
            _config = config;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> Add(User model)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("InsertUser", new { model.FirstName,model.LastName,model.DateOfBirth, model.Email, model.Username });
                message = "User created successfully";
                _logger.LogInformation(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }
            return message;
        }

        public async Task<string> Delete(int userID)
        {
            string message = string.Empty;
            try {
                await _dbo.SaveData("DeleteUser", new { UserID = userID });
                message = "User and its enrollment(s) deleted successfully";
                _logger.LogInformation(message);
            } 
            catch (Exception ex) {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }

            return message;
        }

        public async Task<User> FindById(int UserId)
        {
            var list = await _dbo.LoadData<User, dynamic>("FindByUserID", new { UserID = UserId });
            return list.FirstOrDefault();
        }

        public async Task<User> FindByLoginName(string loginname)
        {
            var list = await _dbo.LoadData<User, dynamic>("FindByLoginName", new { LoginName = loginname });
            return list.FirstOrDefault();
        }

        public async Task<List<User>> List()
        {
            List<User>? list = new();
            try
            {
                IEnumerable<User> usersList = await _dbo.LoadData<User, dynamic>("SelectUsers", new { });
                list = usersList.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return list;
        }

        public async Task<string> UpdateProfile(User model)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("UpdateUser", new { model.UserID, model.FirstName, model.LastName, model.DateOfBirth, model.Email, model.Username });
                message = "User profile updated";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }
            return message;
        }
    }
}
