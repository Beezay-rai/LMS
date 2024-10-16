using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Repository;
using LMS.Data;
using LMS.Interface;
using LMS.Models;
using LMS.Repository;

using LMS.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddGoogle(options =>
{
    options.ClientId = "826437202548-fj078fp80th3bchs3jtn3nve1q2pcu7d.apps.googleusercontent.com";
    options.ClientSecret = "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";
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:IssuerSigningKey"])),
        ValidateIssuerSigningKey = true
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse(); // Suppress the default unauthorized response
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                status = false,
                message = "You are not authorized to access this resource."
            });

            return context.Response.WriteAsync(result);
        }
    };
});



builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authorization with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme,
                }
            }, new List<string>()
        }
    });

});


//Register Here
builder.Services.AddTransient<IAccount, AccountRepository>();
builder.Services.AddTransient<IBook, BookRepository>();
builder.Services.AddTransient<IStudent, StudentRepository>();
builder.Services.AddTransient<ICourse, CourseRepository>();
builder.Services.AddTransient<ITransaction, TransactionRepository>();
builder.Services.AddTransient<ICategory, CategoryRepository>();
builder.Services.AddTransient<IDashboard, DashboardRepository>();
builder.Services.AddTransient<IUtility, Utilities>();



var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.AddSerilog(logger);


var app = builder.Build();

SeedDatabase.Execute(app.Services.GetRequiredService<IConfiguration>(), app.Services);


logger.Information("LMS App Running !");




app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors(options => options
.AllowAnyOrigin()
.AllowAnyHeader()
.AllowAnyMethod()
);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


#region Create Role and  initial User Register

void CreateRolesAndAdministrator(IServiceProvider serviceProvider)
{
    var roleNames = new List<string>()
    {
        "Administrator",
        "SuperAdmin",
        "Admin",
        "User"
    };

    //Creating roles
    foreach (var role in roleNames) { CreateRole(serviceProvider, role); }

    //Administrator User Setup
    const string administratorUserEmail = "Administrator@gmail.com";
    const string administratorPwd = "Administrator@4744";
    AddUserToRole(serviceProvider, new ApplicationUser()
    {
        FirstName = "Yuki",
        LastName = "Rei",
        Email = administratorUserEmail,
        UserName = administratorUserEmail,
        EmailConfirmed = true,
        LockoutEnabled = false,
        Active = true,
    }, administratorPwd, "Administrator");



}
//<Summary>
//Create a role if not exists
//</Summary>
/// <param name="serviceProvider">Service Provider</param>
/// <param name="roleName">Role Name</param>
void CreateRole(IServiceProvider serviceProvider, string roleName)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roleExists = roleManager.RoleExistsAsync(roleName);
    roleExists.Wait();
    if (roleExists.Result) return;
    var roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
    roleResult.Wait();
}

//<summary>
//Add user to a role if the user exits,otherwise create the user and adds him to the role
//</summary>
void AddUserToRole(IServiceProvider serviceProvider, ApplicationUser user, string userPwd, string roleName)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    Task<ApplicationUser> checkAppUser = userManager.FindByEmailAsync(user.Email);
    checkAppUser.Wait();
    ApplicationUser appUser = checkAppUser.Result;
    if (checkAppUser.Result == null)
    {
        Task<IdentityResult> taskCreateAppUser = userManager.CreateAsync(user, userPwd);
        if (taskCreateAppUser.Result.Succeeded)
        {
            appUser = user;
        }
    }
    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(appUser, roleName);
    newUserRole.Wait();
}
#endregion

