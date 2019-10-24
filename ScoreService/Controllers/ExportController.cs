using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ScoreService.Services;

namespace ScoreService.Controllers
{
    [Route("export")]
    public class ExportController : ControllerBase
    {
        private readonly IExportServie _exportServie;

        public ExportController(IExportServie exportServie)
        {
            _exportServie = exportServie;
        }

        [AllowAnonymous]
        [Route("report")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _exportServie.GetReportAsync();
            return File(result.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{result.Name}.xlsx");
        }
    }
}