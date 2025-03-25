using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.User
{
    public class UpdateProfileDTO
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [StringLength(100, ErrorMessage = "Business name cannot exceed 100 characters.")]
        public string? Business { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [Range(typeof(DateTime), "1/1/1900", "12/31/9999", ErrorMessage = "Invalid Date of Birth.")]
        public DateTime? DateOfBirth { get; set; }
    }
}