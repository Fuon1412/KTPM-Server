using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Order;
using Server.Enums.ErrorCodes;
using Server.Interfaces.IServices;
using Server.Middlewares;
using Server.Models.Order;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet] //should be remove in the future
        public async Task<IActionResult> GetOrder(Guid orderID)
        {
            try
            {
                var order = await _orderService.GetOrderAsync(orderID);

                return Ok(order);
            }
            catch (OrderExceptions ex)
            {
                return ex.ErrorCode switch
                {
                    OrderErrorCode.OrderNotFound => BadRequest(new
                    {
                        message = "Order not found"
                    }),
                    _ => BadRequest(new { message = "Unexpected Error" })
                };
            }
        }

        [HttpGet("all-order")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetListOrders()
        {
            try
            {
                var orders = await _orderService.GetListOrdersAsync();

                return Ok(orders);

            }
            catch (OrderExceptions ex)
            {
                return ex.ErrorCode switch
                {
                    OrderErrorCode.OrderNotFound => BadRequest(new
                    {
                        message = "Order not found"
                    }),
                    _ => BadRequest(new { message = "Unexpected Error" })
                };
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO request)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c=>
                    c.Type == ClaimTypes.NameIdentifier ||
                    c.Type == "sub" ||
                    c.Type == "userId" ||
                    c.Type == "id")?.Value;

                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    return BadRequest(new { message = "User identification failed" });
                }

                request.UserId = userGuid;
                var order = await _orderService.CreateOrder(request);
                return Ok(order);
            }
            catch (OrderExceptions ex)
            {
                return ex.ErrorCode switch
                {
                    OrderErrorCode.OrderNotFound => BadRequest(new
                    {
                        message = "Order not found"
                    }),
                    OrderErrorCode.ProductNotFound => BadRequest(new
                    {
                        message = "One or more products not found"
                    }),
                    _ => BadRequest(new { message = "Unexpected Error" })

                };
            }
        }
    }
}

