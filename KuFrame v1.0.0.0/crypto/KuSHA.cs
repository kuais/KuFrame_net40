
using System.IO;
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuSHA1 : ICrypto
    {
        private readonly SHA1 _cryptor = SHA1.Create();
        public byte[] Decrypt(byte[] input) => input;
        public byte[] Encrypt(byte[] input) => _cryptor.ComputeHash(input);
        public byte[] Encrypt(Stream input) => _cryptor.ComputeHash(input);
        public void Dispose() => _cryptor.Dispose();
    }
    public class KuSHA256 : ICrypto
    {
        private readonly SHA256 _cryptor = SHA256.Create();
        public byte[] Decrypt(byte[] input) => input;
        public byte[] Encrypt(byte[] input) => _cryptor.ComputeHash(input);
        public byte[] Encrypt(Stream input) => _cryptor.ComputeHash(input);
        public void Dispose() => _cryptor.Dispose();
    }
    public class KuSHA384 : ICrypto
    {
        private readonly SHA384 _cryptor = SHA384.Create();
        public byte[] Decrypt(byte[] input) => input;
        public byte[] Encrypt(byte[] input) => _cryptor.ComputeHash(input);
        public byte[] Encrypt(Stream input) => _cryptor.ComputeHash(input);
        public void Dispose() => _cryptor.Dispose();
    }
    public class KuSHA512 : ICrypto
    {
        private readonly SHA512 _cryptor = SHA512.Create();
        public byte[] Decrypt(byte[] input) => input;
        public byte[] Encrypt(byte[] input) => _cryptor.ComputeHash(input);
        public byte[] Encrypt(Stream input) => _cryptor.ComputeHash(input);
        public void Dispose() => _cryptor.Dispose();
    }
}
