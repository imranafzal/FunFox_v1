using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace funfoxApi
{
    public static class Api
    {
        public static void ConfigureApis(this WebApplication app)
        {

            app.MapPost("User/Add", (User model, IUserOperations service) => AddUser(model, service));
            app.MapPost("User/UpdateProfile", (User model, IUserOperations service) => UpdateUserProfile(model, service)).RequireAuthorization();
            app.MapGet("User/FindUserById", (int userId, IUserOperations service) => FindUserById(userId, service)).RequireAuthorization();
            app.MapGet("User/FindUserByLoginName", (string loginName, IUserOperations service) => FindUserByLoginName(loginName, service)).RequireAuthorization();
            app.MapPost("User/Delete", [Authorize(Roles = "Admin")] (int userId, IUserOperations service) => DeleteUser(userId, service)).RequireAuthorization();
            app.MapGet("User/List", [Authorize(Roles = "Admin")] (IUserOperations service) => UsersList(service)).RequireAuthorization();

            app.MapPost("Admin/UpdateProfile", [Authorize(Roles = "Admin")](Admin model, IAdminOperations service) => UpdateAdminProfile(model, service)).RequireAuthorization();
            app.MapPost("Admin/Reset", [Authorize(Roles = "Admin")] (IAdminOperations service) => ResetDashboard(service)).RequireAuthorization();

            app.MapPost("Program/Add", [Authorize(Roles = "Admin")] (DataAccess.Models.Program model, IProgramOperations service) => AddProgram(model, service)).RequireAuthorization();
            app.MapPost("Program/Update", [Authorize(Roles = "Admin")] (DataAccess.Models.Program model, IProgramOperations service) => UpdateProgram(model, service)).RequireAuthorization();
            app.MapPost("Program/Delete", [Authorize(Roles = "Admin")] (int programId, IProgramOperations service) => DeleteProgram(programId, service)).RequireAuthorization();
            app.MapGet("Program/SelectProgram", (int ProgramID,IProgramOperations service) => SelectProgram(ProgramID, service)).RequireAuthorization();
            app.MapGet("Program/List", (IProgramOperations service) => ProgramsList(service)).RequireAuthorization();
            app.MapGet("Program/ActiveList", (IProgramOperations service) => ActiveProgramsList(service)).RequireAuthorization();

            app.MapPost("Class/Add", [Authorize(Roles = "Admin")] (DataAccess.Models.Class model, IClassOperations service) => AddClass(model, service)).RequireAuthorization();
            app.MapPost("Class/Update", [Authorize(Roles = "Admin")] (DataAccess.Models.Class model, IClassOperations service) => UpdateClass(model, service)).RequireAuthorization();
            app.MapPost("Class/Delete", [Authorize(Roles = "Admin")] (int classId, IClassOperations service) => DeleteClass(classId, service)).RequireAuthorization();
            app.MapGet("Class/Get", (int ClassID, IClassOperations service) => GetClass(ClassID, service)).RequireAuthorization();
            app.MapGet("Class/List", (IClassOperations service) => ClassesList(service)).RequireAuthorization();
            app.MapGet("Class/FilteredList", (int ProgramId,IClassOperations service) => FilteredList(ProgramId,service)).RequireAuthorization();

            app.MapPost("Enrollment/EnrollForClass", (int userId, int classId, IEnrollmentOperations service) => EnrollForClass(userId,classId, service)).RequireAuthorization();
            app.MapPost("Enrollment/CancelEnrollment", (int enrollmentID, IEnrollmentOperations service) => CancelEnrollment(enrollmentID, service)).RequireAuthorization();
            app.MapGet("Enrollment/UserEnrolledClasses", (int userID, IEnrollmentOperations service) => UserEnrolledClasses(userID, service)).RequireAuthorization();
            app.MapGet("Enrollment/EnrollmentsList", [Authorize(Roles = "Admin")](IEnrollmentOperations service) => EnrollmentsList(service)).RequireAuthorization();

        }

        private static async Task<IResult> ResetDashboard(IAdminOperations service)
        {
            try
            {
                // method name should be more descriptive
                return Results.Ok(await service.UpdateSettings());
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }


        #region User Operations
        private static async Task<IResult> UsersList(IUserOperations service)
        {
            try
            {
                return Results.Ok(await service.List());
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> DeleteUser(int userId, IUserOperations service)
        {
            try
            {
                return Results.Ok(await service.Delete(userId));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> FindUserByLoginName(string loginName, IUserOperations service)
        {
            try
            {
                return Results.Ok(await service.FindByLoginName(loginName));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> FindUserById(int userId, IUserOperations service)
        {
            try
            {
                return Results.Ok(await service.FindById(userId));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> AddUser(User model, IUserOperations service)
        {
            try
            {
                model.DateOfBirth = DateTime.Now.AddYears(-1 * new Random().Next(45));
                return Results.Ok(await service.Add(model));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> UpdateUserProfile(User model, IUserOperations service)
        {
            try
            {
                return Results.Ok(await service.UpdateProfile(model));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        #endregion

        #region Admin Opertaions

        private static async Task<IResult> UpdateAdminProfile(Admin model, IAdminOperations service)
        {
            try
            {
                return Results.Ok(await service.UpdateProfile(model));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        #endregion

        #region Program Opertaions

        private static async Task<IResult> ActiveProgramsList(IProgramOperations service)
        {
            try
            {
                return Results.Ok(await service.List());
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> SelectProgram(int ProgramID,IProgramOperations service)
        {
            try
            {
                return Results.Ok(await service.GetProgram(ProgramID));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }
        private static async Task<IResult> ProgramsList(IProgramOperations service)
        {
            try
            {
                return Results.Ok(await service.ActiveProgramsList());
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> DeleteProgram(int programId, IProgramOperations service)
        {
            try
            {
                return Results.Ok(await service.Delete(programId));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> AddProgram(DataAccess.Models.Program model, IProgramOperations service)
        {
            try
            {
                return Results.Ok(await service.Add(model));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }


        private static async Task<IResult> UpdateProgram(DataAccess.Models.Program model, IProgramOperations service)
        {
            try
            {
                return Results.Ok(await service.Update(model));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }
        #endregion

        #region Class Operations

        private static async Task<IResult> ClassesList(IClassOperations service)
        {
            try
            {
                return Results.Ok(await service.List());
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> FilteredList(int ProgramId,IClassOperations service)
        {
            try
            {
                return Results.Ok(await service.FilteredList(ProgramId));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> GetClass(int classID, IClassOperations service)
        {
            try
            {
                return Results.Ok(await service.Get(classID));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }


        private static async Task<IResult> DeleteClass(int classId, IClassOperations service)
        {
            try
            {
                return Results.Ok(await service.Delete(classId));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> UpdateClass(Class model, IClassOperations service)
        {
            try
            {
                return Results.Ok(await service.Update(model));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> AddClass(Class model, IClassOperations service)
        {
            try
            {
                return Results.Ok(await service.Add(model));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }


        #endregion

        #region Enrollment Operations
        private static async Task<IResult> EnrollmentsList(IEnrollmentOperations service)
        {
            try
            {
                return Results.Ok(await service.EnrollmentDetailList());
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> UserEnrolledClasses(int userID, IEnrollmentOperations service)
        {
            try
            {
                return Results.Ok(await service.UserEnrolledClasses(userID));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> CancelEnrollment(int enrollmentID, IEnrollmentOperations service)
        {
            try
            {
                return Results.Ok(await service.CancelEnrollment(enrollmentID));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> EnrollForClass(int userId, int classId, IEnrollmentOperations service)
        {
            try
            {
                return Results.Ok(await service.EnrollClass(userId,classId));
            }
            catch (Exception ex)
            {
                //also add logging here
                return Results.Problem(ex.Message);
            }
        }
        #endregion

    }
}
