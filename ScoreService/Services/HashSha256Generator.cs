using System;
using System.Security.Cryptography;
using System.Text;

namespace ScoreService.Services
{
    internal class HashSha256Generator : IHashGenerator
    {
        private readonly HashAlgorithm _hashAlgorithm;

        public HashSha256Generator()
        {
            _hashAlgorithm = SHA256.Create();
        }

        public string ComputeHash(string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return ComputeInternal(source);
        }

        public string ComputePasswordHash(string password, string salt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (salt == null)
                throw new ArgumentNullException(nameof(salt));
            var source = $"{password}:{salt}";
            var hash = ComputeInternal(source);
            return hash;
        }

        private string ComputeInternal(string source)
        {
            var sourceData = Encoding.UTF8.GetBytes(source);
            var hashData = _hashAlgorithm.ComputeHash(sourceData);
            var hash = Convert.ToBase64String(hashData);
            return hash;
        }
    }
}