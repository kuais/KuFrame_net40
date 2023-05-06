using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuDES : ICrypto
    {
        private readonly DES _cryptor = DES.Create();

        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public CipherMode Mode { get; set; } = CipherMode.CBC;
        public PaddingMode Padding { get; set; } = PaddingMode.None;
        

        public KuDES()
        {
            Key = _cryptor.Key;
            IV = _cryptor.IV;
        }
        public KuDES(byte[] key, byte[] iv)
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
                CryptoStream cStream = new CryptoStream(mStream, _cryptor.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
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

    public class KuTDES : ICrypto
    {
        private readonly TripleDES _cryptor = TripleDES.Create();

        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public CipherMode Mode { get => _cryptor.Mode; set => _cryptor.Mode = value; }
        public PaddingMode Padding { get => _cryptor.Padding; set => _cryptor.Padding = value; }

        public KuTDES()
        {
            Key = _cryptor.Key;
            IV = _cryptor.IV;
        }
        public KuTDES(byte[] key, byte[] iv)
        {
            Key = key;
            IV = iv;
        }

        public byte[] Decrypt(byte[] input)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                CryptoStream cStream = new CryptoStream(mStream, _cryptor.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
                cStream.Write(input, 0, input.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
        }
        public byte[] Encrypt(byte[] input)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                CryptoStream cStream = new CryptoStream(mStream, _cryptor.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                cStream.Write(input, 0, input.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
        }

        public byte[] DecryptWeak(byte[] input) => TransformWeak(input, 1);
        public byte[] EncryptWeak(byte[] input) => TransformWeak(input, 0);
        public void Dispose() => _cryptor.Dispose();

        private byte[] TransformWeak(byte[] input, int type)
        {
            object[] param = new object[] { Key, Mode, IV, _cryptor.FeedbackSize, type };
            MethodInfo mi = _cryptor.GetType().GetMethod("_NewEncryptor", BindingFlags.Instance | BindingFlags.NonPublic);
            ICryptoTransform tr = (ICryptoTransform)mi.Invoke(_cryptor, param);
            return tr.TransformFinalBlock(input, 0, input.Length);
        }
    }
}
