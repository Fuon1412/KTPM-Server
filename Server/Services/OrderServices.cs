using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs.Order;
using Server.DTOs.Product;
using Server.Enums.ErrorCodes;
using Server.Interfaces.IServices;
using Server.Middlewares;
using Server.Models.Order;

namespace Server.Services
{
    public class OrderServices : IOrderService
    {
        private readonly DatabaseContext _context;
        private readonly IProductService _productService;

        public OrderServices(DatabaseContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<OrderModel> CreateOrder(CreateOrderDTO orderDTO)
        {
            // Validate products exist first
            foreach (var item in orderDTO.OrderItems)
            {
                var product = await _productService.GetProductAsync(item.Product.Id);
                if (product == null)
                {
                    throw new OrderExceptions(OrderErrorCode.ProductNotFound);
                }

                // Optionally check inventory
                if (product.Stock < item.Quantity)
                {
                    throw new OrderExceptions(OrderErrorCode.InsufficientInventory);
                }
            }

            // Create the order
            var order = new OrderModel
            {
                Id = Guid.NewGuid(),
                ShippingFee = orderDTO.ShippingFee,
                TotalPrice = orderDTO.TotalPrice,
                ShippingInfo = orderDTO.ShippingInfo,
                ShippingStatus = "Pending", // Default status
                PaymentMethod = orderDTO.PaymentMethod,
                PaymentStatus = "Pending", // Default status
                CreatedAt = DateTime.UtcNow,
                Tax = 10,
                Discount = orderDTO.Discount,
                UserId = orderDTO.UserId,
                OrderItems = new List<OrderItemModel>()
            };

            decimal totalPrice = 0m;
            decimal shippingFee = 0m;

            // Create order items referencing existing products
            foreach (var itemDto in orderDTO.OrderItems)
            {
                var product = await _productService.GetProductAsync(itemDto.Product.Id);

                var itemShippingFee = 9.99m;
                var orderItem = new OrderItemModel
                {
                    Id = Guid.NewGuid(),
                    Quantity = itemDto.Quantity,
                    ItemShippingFee = itemShippingFee,
                    ProductId = product.Id,
                    Product = product,
                };

                order.OrderItems.Add(orderItem);

                totalPrice += product.Price * itemDto.Quantity;
                shippingFee += itemShippingFee;

                product.Stock -= itemDto.Quantity;
                await _productService.UpdateProduct(product);
            }
            var tax = Math.Round(totalPrice * 0.10m, 2);
            order.ShippingFee = shippingFee;
            order.TotalPrice = totalPrice - order.Discount + tax + shippingFee;


            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();


            return order;
        }
        public async Task UpdateOrder(Guid orderId,
                         UpdateOrderDTO model)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            order.ShippingStatus = model.ShippingStatus;
            order.PaymentStatus = model.PaymentStatus;
            await _context.SaveChangesAsync();

        }

        public async Task<OrderModel> GetOrderAsync(Guid orderId)
        {
            var order = await _context.Orders
                                        .Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Product)
                                        .FirstOrDefaultAsync(a => a.Id == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            return order;
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderModel>> GetListOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
        }
    }
}
