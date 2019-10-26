using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScoreService.Services;
using ScoreService.ViewModel;

namespace ScoreService.Controllers
{
    [Authorize]
    [Route("team")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IExportServie _exportServie;

        public TeamController(ITeamService teamService, ISessionTokenStorageService tokenStorageService, IExportServie exportServie)
        {
            _exportServie = exportServie;
            _teamService = teamService;
        }

        [HttpGet("score")]
        [SessionTokenFilter]
        public async Task<IActionResult> Score(TeamModel model)
        {
            var categories = await _teamService.GetCategoriesAsync();
            var result = new ScoreModel()
            {
                TeamId = model.Id,
                TeamName = model.Name,
                Address = model.Address,
                Categories = categories.ToList()
            };
            return AutoView(result);
        }

        [HttpPost("score")]
        [SessionTokenFilter]
        public async Task<IActionResult> Score([FromBody]ScoreModel model)
        {
            var login = User.Identity.Name;
            await _teamService.SaveScoreAsync(model, login);
            return Ok("/");
        }

        [AllowAnonymous]
        [HttpGet("reshuffle")]
        public async Task<IActionResult> ReshuffleTeams()
        {

            await _teamService.RemoveBindings();
            await _teamService.BindTeamsToUser();
            var result  = await _exportServie.GetUserTeamsBinds();
            return File(result.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{result.Name}.xlsx");
            
        }
    }
}