using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreService.Services;
using ScoreService.ViewModel;

namespace ScoreService.Controllers
{
    [Route("team")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("score")]
        public async Task<IActionResult> Score(TeamModel model)
        {
            await Task.Yield();
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
        public async Task<IActionResult> Score([FromBody]ScoreModel model)
        {
            var login = User.Identity.Name;
            await _teamService.SaveScoreAsync(model, login);
                
            return Content("/");
        }
    }
}