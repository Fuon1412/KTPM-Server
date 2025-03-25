using Server.Models.User;

namespace Server.Models.Account
{
    public class AccountModel
    {
        public required Guid Id { get; set; }
        public required string  Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public required bool IsActivated { get; set; }
        public required UserModel User { get; set; }
    }
}