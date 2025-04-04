using System.Security.Cryptography;
using System.Text;

namespace Potato.Models
{
    public class Crypto
    {
        public static string GetCrypto(string line)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] arrayBytes = Encoding.UTF8.GetBytes(line);
                byte[] bytesCrypto = md5.ComputeHash(arrayBytes);
                return Convert.ToHexString(bytesCrypto);
            }
        }
    }
}
