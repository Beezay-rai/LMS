using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Repository;
using LMS.Data;
using LMS.Interface;
using LMS.Models;
using LMS.Repository;
using LMS.Services;
using LMS.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Writers;
using Serilog;
using System.IO;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

#region Authentication
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
#endregion


var openApiFile = Path.Combine(builder.Environment.WebRootPath, "api-doc", "openapi.json");
var stream = new FileStream(openApiFile, FileMode.Open);

OpenApiDocument openApiDocument = new OpenApiStreamReader().Read(stream, out var diagnostic);



builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "bearer",
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
builder.Services.AddScoped<IAccount, AccountRepository>();
builder.Services.AddScoped<IBook, BookRepository>();
builder.Services.AddScoped<IStudent, StudentRepository>();
builder.Services.AddScoped<ICourse, CourseRepository>();
builder.Services.AddScoped<ITransaction, TransactionRepository>();
builder.Services.AddScoped<ICategory, CategoryRepository>();
builder.Services.AddScoped<IDashboard, DashboardRepository>();
builder.Services.AddScoped<IUtility, Utilities>();


//builder.Services.AddOpenTelemetry()
//    .ConfigureResource(resource =>
//{
//    resource.AddService("LMS");
//})
//    .WithMetrics(metric =>
//    {
//        metric.AddAspNetCoreInstrumentation();
//        metric.AddHttpClientInstrumentation();
//        metric.AddOtlpExporter();
//    })
//    .WithTracing(tracing =>
//    {
//        tracing.AddAspNetCoreInstrumentation();
//        tracing.AddHttpClientInstrumentation();
//        tracing.AddOtlpExporter();

//    });

builder.Services.AddCors(option =>
{
    option.AddPolicy("LmsReact", policy =>
    {
        policy.WithOrigins(new string[] { "http://localhost:5002" })
        .AllowAnyMethod()
        .AllowCredentials()
        .AllowAnyHeader();
    });
});


#region Logging

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
//builder.Logging.AddOpenTelemetry(logging => { logging.AddOtlpExporter(); });
#endregion


var app = builder.Build();

SeedDatabase.Execute(app.Services.GetRequiredService<IConfiguration>(), app.Services);


logger.Information("LMS App Running !");


app.MapHub<SignalrChatHub>("/chatHub");

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();


app.UseCors("LmsReact");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();




