using Server.Models.User;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Order
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalPrice { get; set; }
        public required string ShippingInfo { get; set; }
        public string? ShippingStatus { get; set; }
        public required string PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }

        public Guid UserId { get; set; } // Foreign key

        public ICollection<OrderItemModel> OrderItems
        {
            get; set;

        } = new List<OrderItemModel>();


    }
}
