using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Enums.Auth;
using Server.Enums.ErrorCodes;
using Server.Helpers;
using Server.Interfaces.IServices;
using Server.Middlewares;
using Server.Models.Account;
using Server.Models.User;

namespace Server.Services
{
    public class AuthService(DatabaseContext context) : IAuthService
    {

        private readonly DatabaseContext _context = context;
        private readonly BcryptService _bcryptService = new();
        //Service for authentication
        public async Task<AccountModel> Login(string email, string password)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email)
                          ?? throw new AuthException(AuthErrorCode.AccountNotExist, "Account does not exist.");

            return account switch
            {
                _ when !_bcryptService.VerifyPassword(password, account.Password)
                    => throw new AuthException(AuthErrorCode.InvalidPassword, "Password is not correct."),

                _ when !account.IsActivated
                    => throw new AuthException(AuthErrorCode.AccountNotActive, "Account is not actived."),

                _ => account
            };
        }



        public async Task Register(string email, string password, RoleCode role)
        {
            // Kiểm tra xem tài khoản đã tồn tại chưa
            if (await _context.Accounts.AnyAsync(a => a.Email == email))
            {
                throw new AuthException(AuthErrorCode.AccountAlreadyExist, "Account already exist.");
            }

            var user = new UserModel
            {
                Id = Guid.NewGuid()
            };

            var account = new AccountModel
            {
                Id = Guid.NewGuid(),
                Email = email,
                IsActivated = false,
                Role = role.ToString(),
                Password = _bcryptService.HashPassword(password),
                User = user
            };

            user.AccountId = account.Id;

            _context.Accounts.Add(account);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }


        public async Task ChangePassword(Guid accountId, string oldPassword, string newPassword)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId) ?? throw new AuthException(AuthErrorCode.AccountNotExist, "Account does not exist.");
            if (!_bcryptService.VerifyPassword(oldPassword, account.Password))
                throw new AuthException(AuthErrorCode.InvalidPassword, "Password is not correct.");

            if (oldPassword == newPassword)
                throw new AuthException(AuthErrorCode.OldPasswordMatching, "New password must be different from old password.");

            account.Password = _bcryptService.HashPassword(newPassword);
            await _context.SaveChangesAsync();
        }


        //Active account service
        public async Task<string> GetActiveCode(string email, string password)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email)
                          ?? throw new AuthException(AuthErrorCode.AccountNotExist, "Account does not exist.");

            if (!_bcryptService.VerifyPassword(password, account.Password))
                throw new AuthException(AuthErrorCode.InvalidPassword, "Password is not correct.");

            if (account.IsActivated)
                throw new AuthException(AuthErrorCode.AccountAlreadyActivated, "Account is already activated.");

            var existingCode = await _context.ActivationCodes.FirstOrDefaultAsync(a => a.AccountId == account.Id);
            DateTime currentTime = DateTime.UtcNow;
            string newCode;

            if (existingCode != null)
            {
                TimeSpan timeSinceLastRequest = currentTime - existingCode.LastRequestedTime;
                if (timeSinceLastRequest.TotalSeconds < 60)
                    throw new AuthException(AuthErrorCode.RequestTooFrequent, "Request too frequent.");

                existingCode.Code = ActiveCodeHelper.GenerateActivationCode(account.Id);
                existingCode.ExpiryTime = currentTime.AddMinutes(10);
                existingCode.LastRequestedTime = currentTime;
                newCode = existingCode.Code;  // Lưu mã mới vào biến để trả về
            }
            else
            {
                newCode = ActiveCodeHelper.GenerateActivationCode(account.Id);  // Tạo mã trước
                var activationEntry = new ActivationCodeModel
                {
                    AccountId = account.Id,
                    Code = newCode,  // Gán mã vừa tạo vào đây
                    ExpiryTime = currentTime.AddMinutes(10),
                    LastRequestedTime = currentTime
                };
                _context.ActivationCodes.Add(activationEntry);
            }

            await _context.SaveChangesAsync();  // Lưu thay đổi vào DB
            return newCode;  // Trả về mã vừa tạo
        }

        public async Task ActivateAccount(string activationCode)
        {
            var activationEntry = await _context.ActivationCodes.FirstOrDefaultAsync(a => a.Code == activationCode) ?? throw new AuthException(AuthErrorCode.InvalidActivationCode, "Activation code is not valid.");
            if (activationEntry.ExpiryTime < DateTime.UtcNow)
                throw new AuthException(AuthErrorCode.ActivationCodeExpired, "Activation code is expired.");

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == activationEntry.AccountId) ?? throw new AuthException(AuthErrorCode.AccountNotExist, "Account does not exist.");
            account.IsActivated = true;
            _context.ActivationCodes.Remove(activationEntry);
            await _context.SaveChangesAsync();
        }

        public Task<UserModel?> GetUser(Guid accountId)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.AccountId == accountId);
            return user ?? throw new AuthException(AuthErrorCode.UserNotFound, "User not found.");
        }
    }
}
