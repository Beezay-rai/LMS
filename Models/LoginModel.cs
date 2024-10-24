using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class SignUpModel
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
    public class GETSignUpModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }

    }

    public class LoginModel
    {

        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class ApiResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; } = new object();
    }


    public class LoginResponse
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime Expiration { get; set; }
        public string Role { get; set; }
    }

}