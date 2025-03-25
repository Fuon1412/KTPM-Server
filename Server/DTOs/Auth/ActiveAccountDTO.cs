using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Auth
{
    public class ActiveAccountDTO
    {
        [MaxLength(6, ErrorMessage = "Active code must be 6 characters long.")]
        public required string ActiveCode { get; set; }
    }
}