using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreService.Dto;
using ScoreService.Entities;
using ScoreService.Infrastructure;

namespace ScoreService.Services
{
    public interface IUserService
    {
        Task SaveUser(string login, string password);
        Task<bool> IsUserExistAsync(string login, string password);
        Task ChangePassword(string login, string newPassword);
        Task<ICollection<TeamDto>> GetTeamsAsync(string login);
    }

    class UserService : IUserService
    {
        private readonly IHashGenerator _hashGenerator;
        private readonly SSDbContext _context;

        public UserService(IHashGenerator hashGenerator, SSDbContext context)
        {
            _hashGenerator = hashGenerator;
            _context = context;
        }


        //public async Task<ICollection<TeamEntity>>

        public async Task<bool> IsUserExistAsync(string login, string password)
        {
            var user = await _context.Set<UserEntity>().FirstOrDefaultAsync(p => p.Login == login);
            if (user == null)
                return false;
            var paswordHash = _hashGenerator.ComputePasswordHash(password, user.PasswordSalt);
            if (string.Equals(paswordHash, user.PasswordHash))
            {
                return true;
            }

            return false;
        }

        public async Task SaveUser(string login, string password)
        {

            var salt = Guid.NewGuid().ToString("N");
            var passwordHash = _hashGenerator.ComputePasswordHash(password, salt);

            await _context.AddAsync(new UserEntity()
            {
                Login = login,
                PasswordHash = passwordHash,
                PasswordSalt = salt
            });

            await _context.SaveChangesAsync();
        }

        public async Task ChangePassword(string login, string newPassword)
        {
            var user = await _context.Set<UserEntity>().FirstOrDefaultAsync(p => p.Login == login);
            var salt = Guid.NewGuid().ToString("N");
            var passwordHash = _hashGenerator.ComputePasswordHash(newPassword, salt);

            user.PasswordSalt = salt;
            user.PasswordHash = passwordHash;

            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<TeamDto>> GetTeamsAsync(string login)
        {
            var teams =  await _context.Set<TeamEntity>().Where(p=>p.User.Login == login && !p.IsScored).Select(p=> new TeamDto()
            {
                Id = p.Id,
                Name = p.Name,
                IsScored = p.IsScored
            }).ToListAsync();
            return teams;
        }
    }


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

    /// <summary>
    ///     Генератор хеш-а для безопасности.
    /// </summary>
    public interface IHashGenerator
    {
        /// <summary>
        ///     Вычислить криптоустойчивый хеш для защиты данных.
        /// </summary>
        /// <param name="source">Исходная строка для вычисления хеша.</param>
        /// <returns>Строка, содержащая хеш в BASE-64.</returns>
        string ComputeHash(string source);

        /// <summary>
        ///     Вычислить криптоустойчивый хеш для пароля пользователя.
        /// </summary>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="salt">Соль для генерации хеша.</param>
        /// <returns>Строка, содержащая хеш в BASE-64.</returns>
        string ComputePasswordHash(string password,string salt);
    }
}