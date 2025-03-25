namespace Server.Enums.ErrorCodes
{
    public enum AuthErrorCode
    {
        AccountNotExist,
        AccountAlreadyExist,
        TokenExpired,
        InvalidPassword,
        AccountNotActive,
        PasswordsNotMatch,
        OldPasswordMatching,
        AccountAlreadyActivated,
        InvalidActivationCode,
        ActivationCodeExpired,
        RequestTooFrequent,
        UserNotFound,
        UnknownError
    }
}
