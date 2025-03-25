using Server.Enums.ErrorCodes;

namespace Server.Middlewares
{
    public class UserException(UserErrorCode errorCode, string message) : Exception(message)
    {
        public UserErrorCode ErrorCode { get; set; } = errorCode;
    }
}