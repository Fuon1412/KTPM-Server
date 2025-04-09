using Server.Enums.ErrorCodes;

namespace Server.Middlewares
{
    public class OrderExceptions : Exception
    {
        public OrderErrorCode ErrorCode { get; }

        public OrderExceptions(OrderErrorCode errorCode)
            : base(GetDefaultMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        public OrderExceptions(OrderErrorCode errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public OrderExceptions(OrderErrorCode errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        private static string GetDefaultMessage(OrderErrorCode errorCode)
        {
            return errorCode switch
            {
                OrderErrorCode.ProductNotFound => "One or more products in the order were not found",
                OrderErrorCode.InsufficientInventory => "Insufficient inventory for one or more products",
                OrderErrorCode.InvalidOrderData => "The order data provided is invalid",
                OrderErrorCode.OrderNotFound => "The requested order was not found",
                OrderErrorCode.PaymentFailed => "Payment processing failed",
                _ => "An error occurred processing the order"
            };
        }
    }
}