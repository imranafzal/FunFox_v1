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
    public class EnrollmentOperations : IEnrollmentOperations
    {
        private readonly ISqlDataAccess _dbo;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private ILogger<EnrollmentOperations> _logger;

        public EnrollmentOperations(ISqlDataAccess dbo, IConfiguration config, IMemoryCache cache, ILogger<EnrollmentOperations> logger)
        {
            _dbo = dbo;
            _config = config;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> CancelEnrollment(int enrollmentID)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("CancelEnrollment", new { EnrollmentID = enrollmentID });
                message = "Enrollment cancelled for the class";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }

            return message;
        }

        public async Task<string> EnrollClass(int userId, int classId)
        {
            string message = string.Empty;
            try
            {
                await _dbo.SaveData("EnrollForClass", new { UserID = userId, ClassID= classId});
                message = "Enrollment done for class successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = ex.Message;
            }

            return message;
        }

        public async Task<List<EnrollmentDetails>> UserEnrolledClasses(int userId)
        {
            List<EnrollmentDetails>? list = new();
            try
            {

                IEnumerable<EnrollmentDetails> details = await _dbo.LoadData<EnrollmentDetails, dynamic>("GetUserEnrolledClassesWithDetails", new { UserID = userId });
                list = details.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return list;
        }

        public async Task<List<EnrollmentDetails>> EnrollmentDetailList()
        {
            List<EnrollmentDetails>? list = new();
            try
            {
                IEnumerable<EnrollmentDetails> details = await _dbo.LoadData<EnrollmentDetails, dynamic>("ListEnrolledUsersWithClassDetails", new { });
                list = details.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return list;
        }

    }
}
