namespace Ku.crypto
{
    public interface ICrypto
    {
        byte[] Encrypt(byte[] input);
        byte[] Decrypt(byte[] input);
    }
}