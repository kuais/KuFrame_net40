using System;

namespace Ku.crypto
{
    public interface ICrypto: IDisposable
    {
        byte[] Encrypt(byte[] input);
        byte[] Decrypt(byte[] input);
    }

    public static class CryptoType
    {
        public const string AES = "AES";
        public const string DES = "DES";
        public const string TDES = "TDES";
        public const string MD5 = "MD5";
        public const string RSA = "RSA";
        public const string SHA1 = "SHA1";
        public const string SHA256 = "SHA256";
        public const string SHA384 = "SHA384";
        public const string SHA512 = "SHA512";
    }
}