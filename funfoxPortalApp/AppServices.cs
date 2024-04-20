using DataAccess.Data;
using DataAccess.DbAccess;

namespace funfoxApi
{
    public static class AppServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
            builder.Services.AddSingleton<IUserOperations, UserOperations>();
            builder.Services.AddSingleton<IAdminOperations, AdminOperations>();
            builder.Services.AddSingleton<IProgramOperations, ProgramOperations>();
            builder.Services.AddSingleton<IClassOperations, ClassOperations>();
            builder.Services.AddSingleton<IEnrollmentOperations, EnrollmentOperations>();
        }
    }
}
