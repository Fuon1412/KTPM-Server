namespace Server.Models.Product
{
    public class ProductModel
    {
        /// <example> 12312321 </example>
        public Guid Id { get; set; }
        /// <example> Test </example>
        public required string Name { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
        public int Stock { get; set; }

    }
}
