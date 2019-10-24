using System.Threading.Tasks;

namespace ScoreService
{
    public interface IAppInitializator
    {
        Task PrepareAsync();
    }
}