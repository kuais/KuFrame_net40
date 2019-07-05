using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuMD5 : ICrypto
    {
        private MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();

        public byte[] Decrypt(byte[] input)
        {
            return input;
        }

        public byte[] Encrypt(byte[] input)
        {
            return provider.ComputeHash(input);
        }
    }
}
