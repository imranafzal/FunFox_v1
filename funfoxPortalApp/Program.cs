//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using funfoxApi;
using funfoxApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme 
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAuthorization();
builder.Services
    .AddIdentityApiEndpoints<IdentityUser>(options => 
        { 
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = true;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 5;
        }
    )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

////configured my own services using following extension method 
builder.ConfigureServices();

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Services.AddMemoryCache();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.MapIdentityApi<IdentityUser>();

//app.UseHttpsRedirection();

app.UseAuthorization();


app.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity.Name}")
    .RequireAuthorization();

//app.MapGet("GetUserRoles", (string userId) => {

//    using (var scope = app.Services.CreateScope())
//    {
//        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
//        var roles = userManager.GetRolesAsync(new IdentityUser {UserName = userId });

//        return roles;
//    }

//});

//configured api methods using following extension method 
app.ConfigureApis();

using (var scope = app.Services.CreateScope()) 
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] {"Admin","Member" };

    foreach (var role in roles) 
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "portaladmin@gmail.com";
    string password = "12345678";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        try
        {
            var user = new IdentityUser();
            user.UserName = email;
            user.Email = email;

            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, "Admin");
        }
        catch (Exception ex)
        {
            Console.Write(ex);
        }
    }

}


app.Run();

