using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public interface IEnrollmentOperations
    {
        Task<string> EnrollClass(int userId, int classId);

        Task<string> CancelEnrollment(int EnrollmentID);

        Task<List<EnrollmentDetails>> EnrollmentDetailList();

        Task<List<EnrollmentDetails>> UserEnrolledClasses(int userId);

    }
}
