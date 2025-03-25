using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Auth
{
    public class UpdateDTO
    {
        public required string OldPassword { get; set; }
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public required string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public required string ConfirmPassword { get; set; }
    }
}