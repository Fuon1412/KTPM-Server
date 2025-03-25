using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Auth
{
    public class RegisterDTO
    {
        [EmailAddress]
        public required string Email { get; set; }
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public required string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public required string ConfirmPassword { get; set; }
        public required int Role { get; set; }
    }
}