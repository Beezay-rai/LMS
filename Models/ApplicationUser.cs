using Microsoft.AspNetCore.Identity;

namespace LMS.Models
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
    }

    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }

}
