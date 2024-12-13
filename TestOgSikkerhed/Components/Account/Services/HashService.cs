using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace TestOgSikkerhed.Components.Utilities
{
    public class SecureHashingService
    {
        // Generate a shared salt for the application
        private static readonly byte[] SharedSalt = InitializeSharedSalt();

        private static byte[] InitializeSharedSalt()
        {
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static class HashingAlgorithms
        {
            // SHA256 Hashing
            public static string ComputeSHA256(string input)
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                    return Convert.ToBase64String(hashBytes);
                }
            }

            // HMAC-SHA256 Hashing
            public static string ComputeHMACSHA256(string input, byte[] secretKey)
            {
                using (var hmac = new HMACSHA256(secretKey))
                {
                    byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                    return Convert.ToBase64String(hashBytes);
                }
            }

            // PBKDF2 Hashing
            public static string ComputePBKDF2(string input, byte[] salt, int iterations = 15000, int hashSize = 32)
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(input, salt, iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hashBytes = pbkdf2.GetBytes(hashSize);
                    return Convert.ToBase64String(hashBytes);
                }
            }

            // Bcrypt Hashing
            public static string ComputeBcrypt(string input, int workFactor = 12)
            {
                return BCrypt.Net.BCrypt.HashPassword(input, workFactor);
            }

            // Verify Bcrypt Hash
            public static bool VerifyBcrypt(string input, string storedHash)
            {
                return BCrypt.Net.BCrypt.Verify(input, storedHash);
            }
        }

        // Generic Hashing Utilities
        public T ApplySHA256<T>(T input)
        {
            if (input is string stringInput)
            {
                byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(stringInput));
                return (T)(object)Convert.ToBase64String(hashBytes);
            }
            else if (input is byte[] byteInput)
            {
                byte[] hashBytes = SHA256.HashData(byteInput);
                return (T)(object)Convert.ToBase64String(hashBytes);
            }
            throw new ArgumentException("Unsupported input type for hashing.");
        }

        public T ApplyHMAC<T>(T input, byte[] secretKey)
        {
            using (var hmac = new HMACSHA256(secretKey))
            {
                if (input is string stringInput)
                {
                    byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringInput));
                    return (T)(object)Convert.ToBase64String(hashBytes);
                }
                else if (input is byte[] byteInput)
                {
                    byte[] hashBytes = hmac.ComputeHash(byteInput);
                    return (T)(object)Convert.ToBase64String(hashBytes);
                }
            }
            throw new ArgumentException("Unsupported input type for HMAC.");
        }

        public T ApplyPBKDF2<T>(T input, byte[] salt, int iterations = 15000)
        {
            if (input is string stringInput)
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(stringInput, salt, iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hashBytes = pbkdf2.GetBytes(32);
                    return (T)(object)Convert.ToBase64String(hashBytes);
                }
            }
            else if (input is byte[] byteInput)
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(byteInput, salt, iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hashBytes = pbkdf2.GetBytes(32);
                    return (T)(object)Convert.ToBase64String(hashBytes);
                }
            }
            throw new ArgumentException("Unsupported input type for PBKDF2.");
        }

        public T ApplyBcrypt<T>(T input, int workFactor = 12)
        {
            if (input is string stringInput)
            {
                return (T)(object)BCrypt.Net.BCrypt.HashPassword(stringInput, workFactor);
            }
            else if (input is byte[] byteInput)
            {
                string base64Input = Convert.ToBase64String(byteInput);
                return (T)(object)BCrypt.Net.BCrypt.HashPassword(base64Input, workFactor);
            }
            throw new ArgumentException("Unsupported input type for Bcrypt.");
        }

        public bool VerifyBcrypt<T>(T input, string storedHash)
        {
            if (input is string stringInput)
            {
                return BCrypt.Net.BCrypt.Verify(stringInput, storedHash);
            }
            else if (input is byte[] byteInput)
            {
                string base64Input = Convert.ToBase64String(byteInput);
                return BCrypt.Net.BCrypt.Verify(base64Input, storedHash);
            }
            throw new ArgumentException("Unsupported input type for Bcrypt verification.");
        }
    }
}
