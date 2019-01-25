using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuMD5 : ICrypto
    {
        public byte[] Decrypt(byte[] input)
        {
            return input;
        }

        public byte[] Encrypt(byte[] input)
        {
            return new MD5CryptoServiceProvider().ComputeHash(input);
        }
    }
}
