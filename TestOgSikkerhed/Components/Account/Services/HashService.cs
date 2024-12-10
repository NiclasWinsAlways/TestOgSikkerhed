using System;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
namespace TestOgSikkerhed.Components.Account.Services
{
    public class HashingService
    {
        /// <summary>
        /// Generates a random salt for hashing.
        /// </summary>
        /// <returns>A byte array containing the generated salt.</returns>
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        /// <summary>
        /// Hashes the given input string using Argon2id and the provided salt.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <param name="salt">The salt to use for hashing.</param>
        /// <returns>A byte array containing the hashed value.</returns>
        public byte[] StringHashingWithSalt(string input, byte[] salt)
        {
            Argon2id argon2 = new(Encoding.UTF8.GetBytes(input))
            {
                Salt = salt,
                DegreeOfParallelism = 4, // Number of threads
                Iterations = 2,         // Number of iterations
                MemorySize = 1024       // Memory size in KB
            };

            byte[] hash = argon2.GetBytes(32); // Generate a 32-byte hash
            return hash;
        }
    }
}