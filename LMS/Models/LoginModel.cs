using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class LoginModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

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
