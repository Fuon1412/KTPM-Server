using Server.Models.Product;

namespace Server.Models.Order
{
    public class OrderItemModel
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal ItemShippingFee { get; set; }
        public Guid ProductId { get; set; } // Foreign key
        public Guid OrderId { get; set; } // Foreign key
    }
}
