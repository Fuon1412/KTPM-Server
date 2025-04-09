namespace Server.Enums.ErrorCodes
{
    public enum OrderErrorCode
    {
        OrderAlreadyExisted,
        OrderNotFound,
        ProductNotFound,
        PaymentFailed,
        InvalidOrderData,
        InsufficientInventory,
        UnknownError
    }
}
