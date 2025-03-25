using Server.Enums.ErrorCodes;

namespace Server.Middlewares
{
    public class AuthException(AuthErrorCode errorCode, string message) : Exception(message)
    {
        public AuthErrorCode ErrorCode { get; set; } = errorCode;

    }
}