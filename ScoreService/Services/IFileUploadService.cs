using System.Threading.Tasks;
using ScoreService.ViewModel;

namespace ScoreService.Services
{
    public interface IFileUploadService
    {
        Task UploadAsync(FileLoadType fileLoadType, byte[] content);
    }
}