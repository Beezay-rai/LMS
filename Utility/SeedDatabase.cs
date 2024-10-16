using LMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Utility
{
    public static class SeedDatabase
    {

        public static void Execute(IConfiguration config,IServiceProvider serviceProvider)
        {
            var logger =serviceProvider.GetService<ILogger>();
            try
            {
                //logger.LogInformation("Starting database seeding...");

                var adminCred = new AdminCred();

                config.GetSection("AdminCred").Bind(adminCred);
                if (adminCred != null && adminCred.Username != null && adminCred.Password != null  )
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                }

            }
            catch (Exception ex)
            {
                
            }
        
        }


        protected class AdminCred
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }

  
}
