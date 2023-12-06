using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Repository;
using LMS.Data;
using LMS.Interface;
using LMS.Models;
using LMS.Repository;
using LMS.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = false;
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:IssuerSigningKey"])),
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddEndpointsApiExplorer();
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
<<<<<<< HEAD
builder.Services.AddTransient<ICourse, CourseRepository>();
builder.Services.AddTransient<ITransaction, TransactionRepository>();
=======
builder.Services.AddTransient<IFaculty, FacultyRepository>();
builder.Services.AddTransient<IIssueBook, IssueBookRepository>();
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
builder.Services.AddTransient<ICategory, CategoryRepository>();
builder.Services.AddTransient<IDashboard, DashboardRepository>();
builder.Services.AddTransient<ICommon, CommonRepository>();

var app = builder.Build();
app.Logger.LogInformation("Initialize the app");

//Roles and Users
using (var scope = app.Services.CreateScope())
{
    CreateRolesAndAdministrator(scope.ServiceProvider);
    AddRequiredData(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors(options => options
.WithOrigins(builder.Configuration.GetSection("AllowOrigins").Get<List<string>>().ToArray())
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
);
//}
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


#region Create Role and  initial User Register

void CreateRolesAndAdministrator(IServiceProvider serviceProvider)
{
    var roleNames = new List<string>()
    {
        UserRoles.Administrator,
        UserRoles.SuperAdmin,
        UserRoles.Admin,
        UserRoles.User
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
<<<<<<< HEAD
        Active = true,
    }, administratorPwd, UserRoles.Administrator);


=======
        Active = true,  
    }, administratorPwd, UserRoles.Administrator);

 
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f

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


#region Required Data For Lms
void AddRequiredData(IServiceProvider serviceProvider)
{
    using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

    #region Gender
    var listOfGender = new List<Gender>
    {
       new Gender(){Id = 1,Name="Male"},
       new Gender(){Id = 2,Name="Female"}
    };
<<<<<<< HEAD
    using (var transaction = context.Database.BeginTransaction())
=======
    using(var transaction = context.Database.BeginTransaction())
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
    {
        try
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Gender] On");
<<<<<<< HEAD
            foreach (var gender in listOfGender)
=======
            foreach(var gender in listOfGender)
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
            {
                if (context.Gender.Any(x => x.Id == gender.Id)) continue;
                context.Gender.Add(gender);
                context.SaveChanges();
            }
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Gender] Off");
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
        }

    }
    #endregion
}

#endregion