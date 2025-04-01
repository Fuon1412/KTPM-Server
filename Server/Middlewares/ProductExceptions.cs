using Server.Enums.ErrorCodes;

namespace Server.Middlewares
{
    public class ProductExceptions(ProductErrorCode errorCode, string message) : Exception(message)
    {
        public ProductErrorCode ErrorCode { get; set; } = errorCode;
    }
}
