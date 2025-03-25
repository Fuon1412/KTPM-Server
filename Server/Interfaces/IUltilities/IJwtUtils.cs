namespace Server.Interfaces.IUltilities
{
    public interface IJwtUtils
    {
        string GenerateToken(Guid accountId, string role, Guid userId);
    }
}