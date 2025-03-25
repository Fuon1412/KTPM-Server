namespace Server.Interfaces.IServices
{
    public interface IBcryptService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hashedPassword);    
    }
}
