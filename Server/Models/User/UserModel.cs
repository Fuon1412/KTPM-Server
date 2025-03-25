using Server.Models.Account;

namespace Server.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Business { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid AccountId { get; set; } //foreign key
    }
}