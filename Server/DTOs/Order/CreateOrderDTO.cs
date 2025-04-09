using Server.Models.Order;
using System.ComponentModel.DataAnnotations;

namespace Server.DTOs.Order
{
    public class CreateOrderDTO
    {
        public decimal ShippingFee { get; set; }
        public decimal TotalPrice { get; set; }
        public required string ShippingInfo { get; set; }
        public required string PaymentMethod { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public Guid UserId { get; set; } // Foreign key

        // Order items - essential for creating an order
        [Required(ErrorMessage = "At least one item is required")]
        public required List<OrderItemModel> OrderItems { get; set; }
    }

}
