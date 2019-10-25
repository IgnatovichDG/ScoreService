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
        private readonly ISessionTokenStorageService _tokenStorageService;
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService, ISessionTokenStorageService tokenStorageService)
        {
            _teamService = teamService;
            _tokenStorageService = tokenStorageService;
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
    }
}