﻿namespace Server.DTOs.Product
{
    public class GetProductDTO
    {
        public required string Name { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
        public int Stock { get; set; }
    }
}
