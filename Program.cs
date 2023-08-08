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
builder.Services.AddTransient<IAuthor, AuthorRepository>();
builder.Services.AddTransient<IBook, BookRepository>();

var app = builder.Build();
app.Logger.LogInformation("Initialize the app");

//Roles and Users
using (var scope = app.Services.CreateScope())
{
    CreateRolesAndAdministrator(scope.ServiceProvider);
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
        LockoutEnabled = false
    }, administratorPwd, UserRoles.Administrator);

    //SuperAdmin User Setup
    const string superAdminUserEmail = "Superadmin@gmail.com";
    const string superAdminPwd = "Superadmin@4744";
    AddUserToRole(serviceProvider, new ApplicationUser()
    {
        FirstName = "Super",
        LastName = "Admin",
        Email = superAdminUserEmail,
        UserName = superAdminUserEmail,
        EmailConfirmed = true,
        LockoutEnabled = false

    }, superAdminPwd, UserRoles.SuperAdmin);

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
