using System.Security.Cryptography;
using System.Text;

namespace LMS.Crypto
{
    public static class HMACHelper
    {
        const string MySecret = "MySecret";

        internal static string SHA512_ComputeHash(string text)
        {
            var hash = new StringBuilder();
            byte[] secretkeyBytes = Encoding.UTF8.GetBytes(MySecret);
            byte[] inputBytes = Encoding.UTF8.GetBytes(text);
            using (var hmac = new HMACSHA512(secretkeyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashValue);
            }
        }
    }
}
