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
    public class AdminOperations : IAdminOperations
    {
        private readonly ISqlDataAccess _dbo;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private ILogger<AdminOperations> _logger;

        public AdminOperations(ISqlDataAccess dbo, IConfiguration config, IMemoryCache cache,ILogger<AdminOperations> logger)
        {
            _dbo = dbo;
            _config = config;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> UpdateProfile(Admin model)
        {
            string message = string.Empty;
            try {
                await _dbo.SaveData("UpdateAdmin", model);
                message = "Admin profile updated";
            } catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }
            return message;
        }

        public async Task<string> UpdateSettings()
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("ResetForAdmin", new { });
                message = "Data in all tables has been reset except Admin User Account.";
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
