﻿using System.IO;
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuDES : ICrypto
    {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public CipherMode Mode { get; set; } = CipherMode.CBC;
        public PaddingMode Padding { get; set; } = PaddingMode.None;

        public KuDES(byte[] key, byte[] iv)
        {
            this.Key = key;
            this.IV = iv;
        }
        public byte[] Encrypt(byte[] input)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Mode = Mode;
            provider.Padding = Padding;
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            cStream.Write(input, 0, input.Length);
            cStream.FlushFinalBlock();
            return mStream.ToArray();
        }

        public byte[] Decrypt(byte[] input)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Mode = Mode;
            provider.Padding = Padding;
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
            cStream.Write(input, 0, input.Length);
            cStream.FlushFinalBlock();
            return mStream.ToArray();
        }
    }
}
