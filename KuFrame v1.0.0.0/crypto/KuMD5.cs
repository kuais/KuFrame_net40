using System.IO;
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuMD5 : ICrypto
    {
        private readonly MD5 _cryptor = MD5.Create();
        public byte[] Decrypt(byte[] input) => input;
        public byte[] Encrypt(byte[] input) => _cryptor.ComputeHash(input);
        public byte[] Encrypt(Stream input) => _cryptor.ComputeHash(input);
        public void Dispose() => _cryptor.Dispose();
        public string FileMd5(Stream input)
        {
            var data = Encrypt(input);
            return System.BitConverter.ToString(data).Replace("-", "");
        }
    }
}
