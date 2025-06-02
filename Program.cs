
using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Repository;
using LMS.Data;
using LMS.Filters;
using LMS.Interfaces;
using LMS.Models;
using LMS.Models.Settings;
using LMS.Repository;
using LMS.Security;
using LMS.Services;

using LMS.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
    options.Filters.Add(new CacheFilter());
});


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(Program));


#region Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddScheme<AuthenticationSchemeOptions, BearerAuthHandler>("Bearer", null)
.AddGoogle(options =>
{
    options.ClientId = "826437202548-fj078fp80th3bchs3jtn3nve1q2pcu7d.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

#endregion





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



builder.Services.AddScoped<IAuthenticateRepository, AuthenticateRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();
builder.Services.AddScoped<IRentBookRepository, RentBookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IDashboard, DashboardRepository>();
builder.Services.AddScoped<IUtility, Utilities>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddTransient<IEmailService, EmailService>();

var redisCacheSetting = new RedisCacheSetting();
builder.Configuration.GetSection("RedisCacheSetting").Bind(redisCacheSetting);
builder.Services.AddSingleton(redisCacheSetting);


if (redisCacheSetting.Enable)
{
    IConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                EndPoints = { redisCacheSetting.ConnectionString },
                User = redisCacheSetting.Username,
                Password = redisCacheSetting.Password
            }
        );
    builder.Services.AddStackExchangeRedisCache(option =>
    {
        option.ConnectionMultiplexerFactory = () => Task.FromResult(muxer);
    });
    builder.Services.AddSingleton(muxer);
    builder.Services.AddSingleton<ICacheService, RedisCacheService>();
}

var sendGridSetting = new SendGridSetting();
builder.Configuration.GetSection("SendGridSetting").Bind(sendGridSetting);
builder.Services.AddSingleton(sendGridSetting);



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
        policy.WithOrigins(new string[] { "http://localhost:3000" })
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


app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature == null) return;

        var exception = exceptionHandlerFeature.Error;
        var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();

        var problem = new ProblemDetails()
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Status = 500,
            Title = "Internal Server Error",
        };
        if (env.IsDevelopment())
        {
            problem.Instance = context.Request.Path;
            problem.Detail = exception.Message;
        }


        await context.Response.WriteAsJsonAsync(problem);
    });
});


app.MapHub<SignalrChatHub>("/chatHub");

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseCors("LmsReact");

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
//app.MapFallback(async context =>
//{
//    context.Response.StatusCode = 404;
//    var problem = new ProblemDetails()
//    {
//        Title = "Not Found",
//        Detail = "Requested Service Not Found",
//        Instance = context.Request.Path,
//        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
//        Status = 404,
//    };
//    await context.Response.WriteAsJsonAsync(problem);
//});







app.Run();




