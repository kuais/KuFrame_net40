using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuTDES : ICrypto
    {
        private readonly TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public CipherMode Mode { get => provider.Mode; set => provider.Mode = value; }
        public PaddingMode Padding { get => provider.Padding; set => provider.Padding = value; }

        public KuTDES()
        {
            provider.GenerateKey();
            provider.GenerateIV();
            this.Key = provider.Key;
            this.IV = provider.IV;
        }
        public KuTDES(byte[] key, byte[] iv)
        {
            this.Key = key;
            this.IV = iv;
        }
        public byte[] Encrypt(byte[] input)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                cStream.Write(input, 0, input.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
        }

        public byte[] Decrypt(byte[] input)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
                cStream.Write(input, 0, input.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
        }

        public byte[] DecryptWeak(byte[] input) => TransformWeak(input, 1);
        public byte[] EncryptWeak(byte[] input) => TransformWeak(input, 0);

        private byte[] TransformWeak(byte[] input, int type)
        {
            object[] param = new object[] { Key, Mode, IV, provider.FeedbackSize, type };
            MethodInfo mi = provider.GetType().GetMethod("_NewEncryptor", BindingFlags.Instance | BindingFlags.NonPublic);
            ICryptoTransform tr = (ICryptoTransform)mi.Invoke(provider, param);
            return tr.TransformFinalBlock(input, 0, input.Length);
        }
    }
}
