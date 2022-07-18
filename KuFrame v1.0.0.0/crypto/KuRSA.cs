using System;
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuRSA : ICrypto
    {
        private readonly RSA _cryptor = RSA.Create();
        public bool UseOAEP { get; set; } = false;

        public KuRSA() { }
        public KuRSA(string key) { _cryptor.FromXmlString(key);}

        public byte[] Decrypt(byte[] input) => _cryptor.DecryptValue(input);
        public byte[] Encrypt(byte[] input) => _cryptor.EncryptValue(input);
        public void Dispose() => _cryptor.Dispose();

        public string Sign(byte[] input,string algorithm)
        {
            var rf = new RSAPKCS1SignatureFormatter(_cryptor);
            rf.SetHashAlgorithm(algorithm);
            byte[] output = rf.CreateSignature(input);
            return Convert.ToBase64String(output);
        }
        public bool Verify(byte[] input, string signature, string algorithm)
        {
            RSAPKCS1SignatureDeformatter rf = new RSAPKCS1SignatureDeformatter(_cryptor);
            rf.SetHashAlgorithm(algorithm);
            return rf.VerifySignature(input, Convert.FromBase64String(signature));
        }

        public static RSAKey GenerateKeys()
        {
            RSA cryptor = RSA.Create();
            RSAKey key = new RSAKey();
            key.PrivateKey = cryptor.ToXmlString(false);
            key.PublicKey = cryptor.ToXmlString(true);
            return key;
        }

        public class RSAKey
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }
    }
}
