using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreService.Dto;
using ScoreService.Entities;
using ScoreService.Infrastructure;

namespace ScoreService.Services
{
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
}