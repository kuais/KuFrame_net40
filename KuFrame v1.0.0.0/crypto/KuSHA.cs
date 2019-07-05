
using System.Security.Cryptography;

namespace Ku.crypto
{
    public class KuSHA1 : ICrypto
    {
        public byte[] Decrypt(byte[] input)
        {
            return input;
        }

        public byte[] Encrypt(byte[] input)
        {
            return new SHA1CryptoServiceProvider().ComputeHash(input);
        }
    }
    public class KuSHA256 : ICrypto
    {
        public byte[] Decrypt(byte[] input)
        {
            return input;
        }

        public byte[] Encrypt(byte[] input)
        {
            return new SHA256CryptoServiceProvider().ComputeHash(input);
        }
    }
    public class KuSHA384 : ICrypto
    {
        public byte[] Decrypt(byte[] input)
        {
            return input;
        }

        public byte[] Encrypt(byte[] input)
        {
            return new SHA384CryptoServiceProvider().ComputeHash(input);
        }
    }
    public class KuSHA512 : ICrypto
    {
        public byte[] Decrypt(byte[] input)
        {
            return input;
        }

        public byte[] Encrypt(byte[] input)
        {
            return new SHA512CryptoServiceProvider().ComputeHash(input);
        }
    }
}
