using Server.Models.Account;
using Server.Interfaces.IServices;
using Server.Data;
using Server.DTOs.User;
using Server.Models.User;
using Server.Middlewares;
using Server.Enums.ErrorCodes;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class UserService(DatabaseContext context) : IUserService
    {
        private readonly DatabaseContext _context = context;
        public async Task<AccountModel?> GetAccountAsync(Guid accountId)
        {
            var account = await _context.Accounts
                            .Include(a => a.User)
                            .FirstOrDefaultAsync(a => a.Id == accountId);
            return account ?? throw new UserException(UserErrorCode.AccountNotFound, "Account not found.");
        }

        public async Task<List<AccountModel>> GetAccountsAsync()
        {
            var accounts = await _context.Accounts
                            .Include(a => a.User)        
                            .ToListAsync();
            return accounts ?? throw new UserException(UserErrorCode.UnknownError, "Unknown error.");
        }

        public async Task<GetProfileDTO> GetProfileAsync(Guid accountId)
        {
            var profile = await _context.Accounts
                            .Include(a => a.User)
                            .FirstOrDefaultAsync(a => a.Id == accountId)
                            ?? throw new UserException(UserErrorCode.UserNotFound, "User not found.");
            return new GetProfileDTO
            {
                Email = profile.Email,
                Role = profile.Role,
                FirstName = profile.User?.FirstName ?? string.Empty,
                LastName = profile.User?.LastName ?? string.Empty,
                DateOfBirth = profile.User?.DateOfBirth ?? default,
                Business = profile.User?.Business ?? string.Empty
            };
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users ?? throw new UserException(UserErrorCode.UnknownError, "Unknown error.");
        }

        public async Task UpdateProfileAsync(Guid userId, UpdateProfileDTO model)
        {
            var user = await _context.Users.FindAsync(userId) ?? throw new UserException(UserErrorCode.UserNotFound, "User not found.");

            var userType = typeof(UserModel);
            var dtoType = typeof(UpdateProfileDTO);

            foreach (var prop in dtoType.GetProperties())
            {
                var newValue = prop.GetValue(model);
                if (newValue != null && !(newValue is string str && string.IsNullOrWhiteSpace(str)))
                {
                    var userProp = userType.GetProperty(prop.Name);
                    userProp?.SetValue(user, newValue);
                }
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}