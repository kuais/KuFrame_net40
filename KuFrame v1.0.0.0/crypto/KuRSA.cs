using System;
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuRSA : ICrypto
    {
        private RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
        public bool UseOAEP { get; set; } = false;

        public KuRSA(string key)
        {
            provider.FromXmlString(key);
        }

        public static RSAKey GenerateKeys()
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            RSAKey key = new RSAKey();
            key.PrivateKey = provider.ToXmlString(false);
            key.PublicKey = provider.ToXmlString(true);
            return key;
        }

        public byte[] Decrypt(byte[] input)
        {
            return provider.Decrypt(input, UseOAEP);
        }

        public byte[] Encrypt(byte[] input)
        {
            return provider.Encrypt(input, UseOAEP);
        }

        public string Sign(byte[] input,string algorithm)
        {
            RSAPKCS1SignatureFormatter rf = new RSAPKCS1SignatureFormatter(provider);
            rf.SetHashAlgorithm(algorithm);
            byte[] output = rf.CreateSignature(input);
            return Convert.ToBase64String(output);
        }

        public bool Verify(byte[] input, string signature, string algorithm)
        {
            RSAPKCS1SignatureDeformatter rf = new RSAPKCS1SignatureDeformatter(provider);
            rf.SetHashAlgorithm(algorithm);
            return rf.VerifySignature(input, Convert.FromBase64String(signature));
        }

        public class RSAKey
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }
    }
}
