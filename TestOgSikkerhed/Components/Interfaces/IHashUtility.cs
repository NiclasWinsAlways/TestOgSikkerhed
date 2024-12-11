namespace TestOgSikkerhed.Components.Interfaces
{
    public interface IHashUtility
    {
        string ComputeSHA2(string input);
        byte[] ComputeSHA2(byte[] input);

        string ComputeHMAC(string input, string key = null);
        byte[] ComputeHMAC(byte[] input, byte[] key = null);

        string ComputePBKDF2(string input, string salt = null, int iterations = 10000, int hashSize = 32);
        byte[] ComputePBKDF2(byte[] input, byte[] salt = null, int iterations = 10000, int hashSize = 32);

        string ComputeBcrypt(string input, int workFactor = 10);
        bool VerifyBcrypt(string input, string storedHash);
    }

}