using Server.DTOs.User;
using Server.Models.Account;
using Server.Models.User;

namespace Server.Interfaces.IServices
{
    public interface IUserService
    {
        Task<AccountModel?> GetAccountAsync(Guid accountId);
        Task<GetProfileDTO> GetProfileAsync(Guid userId);
        Task UpdateProfileAsync(Guid userId, UpdateProfileDTO model);
        Task<List<AccountModel>> GetAccountsAsync();
        Task<List<UserModel>> GetUsersAsync();
    }
}