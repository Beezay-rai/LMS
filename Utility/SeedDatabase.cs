using Microsoft.Extensions.Configuration;

namespace LMS.Utility
{
    public static class SeedDatabase
    {

        public static void Execute(IConfiguration config,IServiceProvider provider)
        {
            var logger =provider.GetService<ILogger>();
            try
            {
                logger.LogInformation("Starting database seeding...");

                var adminCred = new AdminCred();

                config.GetSection("AdminCred").Bind(adminCred);
                if (adminCred != null && adminCred.Username != null && adminCred.Password != null  )
                {

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
