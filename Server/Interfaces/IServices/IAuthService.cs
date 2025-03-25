using Server.Enums.Auth;
using Server.Enums.ErrorCodes;
using Server.Models.Account;
using Server.Models.User;

namespace Server.Interfaces.IServices
{
    public interface IAuthService
    {
        Task <AccountModel> Login(string email, string password);
        Task <UserModel?> GetUser(Guid accountId);
        Task Register(string email, string password, RoleCode rolerole);
        Task ChangePassword(Guid accountId, string oldPassword, string newPassword);
        Task<string> GetActiveCode(string email, string password);
        Task ActivateAccount(string activationCode);
    }
}