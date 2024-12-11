using System.Security.Cryptography;
using System.Text;
using TestOgSikkerhed.Components.Interfaces;

namespace TestOgSikkerhed.Components.Utilities
{
    using System.Security.Cryptography;
    using System.Text;

    public class HashUtility : IHashUtility
    {
        private const string DefaultHmacKey = "SecureKey123!";
        private const string DefaultSalt = "DynamicSaltValue";
        private const int DefaultPbkdf2Iterations = 10000;
        private const int DefaultHashSize = 32;

        #region SHA2
        public string ComputeSHA2(string input)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }

        public byte[] ComputeSHA2(byte[] input)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(input);
        }
        #endregion

        #region HMAC
        public string ComputeHMAC(string input, string key = null)
        {
            key ??= DefaultHmacKey;
            var hmacKey = Encoding.UTF8.GetBytes(key);
            using var hmac = new HMACSHA256(hmacKey);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }

        public byte[] ComputeHMAC(byte[] input, byte[] key = null)
        {
            key ??= Encoding.UTF8.GetBytes(DefaultHmacKey);
            using var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(input);
        }
        #endregion

        #region PBKDF2
        public string ComputePBKDF2(string input, string salt = null, int iterations = DefaultPbkdf2Iterations, int hashSize = DefaultHashSize)
        {
            salt ??= DefaultSalt;
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(input, saltBytes, iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(hashSize);
            return Convert.ToBase64String(hash);
        }

        public byte[] ComputePBKDF2(byte[] input, byte[] salt = null, int iterations = DefaultPbkdf2Iterations, int hashSize = DefaultHashSize)
        {
            salt ??= Encoding.UTF8.GetBytes(DefaultSalt);
            using var pbkdf2 = new Rfc2898DeriveBytes(input, salt, iterations, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(hashSize);
        }
        #endregion

        #region Bcrypt
        public string ComputeBcrypt(string input, int workFactor = 10)
        {
            return BCrypt.Net.BCrypt.HashPassword(input, workFactor);
        }

        public bool VerifyBcrypt(string input, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(input, storedHash);
        }
        #endregion
    }
}