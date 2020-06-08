using System.IO;
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
        public byte[] Encrypt(Stream input)
        {
            return provider.ComputeHash(input);
        }
        public string FileMd5(Stream input)
        {
            var data = Encrypt(input);
            return System.BitConverter.ToString(data).Replace("-","");
        }
    }
}
