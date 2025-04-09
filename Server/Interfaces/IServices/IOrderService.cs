using Server.DTOs.Order;
using Server.Models.Order;

namespace Server.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<OrderModel> CreateOrder(CreateOrderDTO orderInfo);
        Task UpdateOrder(Guid orderId,
                         UpdateOrderDTO model
                         );
        Task<OrderModel> GetOrderAsync(Guid orderId);
        Task DeleteOrderAsync(Guid orderId);
        Task<List<OrderModel>> GetListOrdersAsync();
    }
}
