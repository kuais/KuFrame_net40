using System.IO;
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuAES : ICrypto
    {
        private readonly Aes _cryptor = Aes.Create();

        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public CipherMode Mode { get; set; } = CipherMode.CBC;
        public PaddingMode Padding { get; set; } = PaddingMode.Zeros;

        public KuAES()
        {
            Key = _cryptor.Key;
            IV = _cryptor.IV;
        }
        public KuAES(byte[] key, byte[] iv)
        {
            Key = key;
            IV = iv;
        }
        public byte[] Decrypt(byte[] input)
        {
            _cryptor.Mode = Mode;
            _cryptor.Padding = Padding;
            using (MemoryStream mStream = new MemoryStream())
            {
                CryptoStream cStream = new CryptoStream(mStream, _cryptor.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                cStream.Write(input, 0, input.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
        }

        public byte[] Encrypt(byte[] input)
        {
            _cryptor.Mode = Mode;
            _cryptor.Padding = Padding;
            using (MemoryStream mStream = new MemoryStream())
            {
                CryptoStream cStream = new CryptoStream(mStream, _cryptor.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                cStream.Write(input, 0, input.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
        }

        public void Dispose() => _cryptor.Dispose();
    }
}
