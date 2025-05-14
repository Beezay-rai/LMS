using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }


    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }




    public class LoginResponse
    {
        public string Name { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string role { get; set; }
    }

}