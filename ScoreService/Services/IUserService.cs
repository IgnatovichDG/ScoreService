using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreService.Dto;

namespace ScoreService.Services
{
    public interface IUserService
    {
        Task SaveUser(string login, string password);
        Task<bool> IsUserExistAsync(string login, string password);
        Task ChangePassword(string login, string newPassword);
        Task<ICollection<TeamDto>> GetTeamsAsync(string login);
    }
}