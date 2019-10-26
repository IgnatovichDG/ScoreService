using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreService.Dto;
using ScoreService.Entities;

namespace ScoreService.Services
{
    public interface IUserService
    {
        Task SaveUser(string login, string password, string zone);
        Task<UserEntity> IsUserExistAsync(string login, string password);
        Task ChangePassword(string login, string newPassword);
        Task<ICollection<TeamDto>> GetTeamsAsync(string login);
    }
}