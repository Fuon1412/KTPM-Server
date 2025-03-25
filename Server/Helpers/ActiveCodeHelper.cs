using System.Security.Cryptography;
using System.Text;

namespace Server.Helpers
{
    public class ActiveCodeHelper
    {
        public static string GenerateActivationCode(Guid accountId)
        {
            using var sha256 = SHA256.Create();
            
            string input = accountId.ToString() + DateTime.UtcNow.Ticks.ToString();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            string base64Hash = Convert.ToBase64String(hashBytes);
            string activationCode = new string(base64Hash.Where(char.IsLetterOrDigit).ToArray()).Substring(0, 6).ToUpper();

            return activationCode;
        }
    }
}