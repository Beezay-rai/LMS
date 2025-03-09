using LMS.Models;
using Microsoft.AspNetCore.Identity;

namespace LMS.Utility
{
    public static class SeedDatabase
    {
        public static async void Execute(IConfiguration config, IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<ApplicationUser>>();
            try
            {

                var adminCred = new AdminCred();
                config.GetSection("AdminCred").Bind(adminCred);

                if (adminCred != null && adminCred.Username != null && adminCred.Password != null)
                {

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServiceProvider = scope.ServiceProvider;


                        var userManager = scopedServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


                        var existingUser = await userManager.FindByEmailAsync(adminCred.Email);
                        if (existingUser == null)
                        {
                            logger.LogInformation("Starting database seeding...");

                            var AdminUser = new ApplicationUser()
                            {
                                Email = adminCred.Email,
                                UserName = adminCred.Username,
                                Active = true,
                                AccessFailedCount = 0,
                                EmailConfirmed = true,
                                FirstName = "Bijay",
                                LastName = "Rai",
                                LockoutEnabled = false,
                                PhoneNumber = "9865293427",
                                PhoneNumberConfirmed = true,
                            };

                            var result = await userManager.CreateAsync(AdminUser, adminCred.Password);
                            if (result.Succeeded)
                            {
                                logger.LogInformation("Administrator Seeded into Database!");
                            }
                            else
                            {
                                logger.LogError("Failed to seed Administrator into Database!");
                            }
                        }
                      
                    }
                }
                else
                {
                    logger.LogWarning("Admin credentials are missing or incomplete in the configuration.");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database seeding.");
            }
        }




        protected class AdminCred
        {
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }


}
