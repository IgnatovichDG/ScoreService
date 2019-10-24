using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreService.ViewModel;

namespace ScoreService.Services
{
    public interface ITeamService
    {
        Task<bool> IsScoredAsync(long teamId);
        Task<ICollection<ScoreCategoryModel>> GetCategoriesAsync();
        Task SaveScoreAsync(ScoreModel model, string login);
    }
}