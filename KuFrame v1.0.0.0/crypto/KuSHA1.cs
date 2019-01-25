
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuSHA1 : ICrypto
    {
        public byte[] Decrypt(byte[] input)
        {
            return input;
        }

        public byte[] Encrypt(byte[] input)
        {
            return new SHA1CryptoServiceProvider().ComputeHash(input);
        }
    }
}
