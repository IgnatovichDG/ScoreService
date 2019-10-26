using System.Threading.Tasks;
using ScoreService.Dto;

namespace ScoreService.Services
{
    public interface IExportServie
    {
        Task<FileExportDto> GetReportAsync();
        Task<FileExportDto> GetUserTeamsBinds();
    }
}