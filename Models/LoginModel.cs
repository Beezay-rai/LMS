﻿using System.ComponentModel.DataAnnotations;

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
        public string Password { get; set; }
    
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

   


    public class LoginResponse
    {
        public string Name { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime Expiration { get; set; }
    }

}