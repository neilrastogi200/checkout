using System.Security.Cryptography;
using System.Text;

namespace Payment.Gateway.Application.Encryption
{
    public class Key256BitsGenerator : IKeyGenerator
    {   
        public byte[] Generate(string input)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        public int GetKeySize() => 256;
    }
}