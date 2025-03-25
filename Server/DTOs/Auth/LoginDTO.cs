using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Auth
{
    public class LoginDTO
    {
        [EmailAddress]
        public required string Email { get; set; }
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public required string Password { get; set; }
    }
}