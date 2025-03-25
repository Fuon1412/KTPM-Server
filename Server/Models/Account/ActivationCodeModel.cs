namespace Server.Models.Account
{
    public class ActivationCodeModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public required string Code { get; set; }
        public DateTime ExpiryTime { get; set; }
        public DateTime LastRequestedTime { get; set; }
    }
}