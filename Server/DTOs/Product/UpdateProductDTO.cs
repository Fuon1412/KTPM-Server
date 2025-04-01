using System.ComponentModel.DataAnnotations;


namespace Server.DTOs.Product
{
    public class UpdateProductDTO
    {
        [Required(ErrorMessage = "Product's name is required")]
        [StringLength(50, ErrorMessage = "Product's Name cannot exceed 50 characters.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Product's brand is required")]
        public string? Brand { get; set; }

        [StringLength(250, ErrorMessage ="Description cannot exceed 250 characters.")]
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
        public int Stock { get; set; }
    }
}
