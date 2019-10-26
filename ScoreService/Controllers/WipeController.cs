using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreService.Entities;
using ScoreService.Infrastructure;
using ScoreService.Services;
using ScoreService.ViewModel;

namespace ScoreService.Controllers
{
    [Route("iddqd")]
    public class WipeController : ControllerBase
    {
        private readonly SSDbContext _dbContext;
        private readonly ISessionTokenStorageService _tokenStorageService;

        public WipeController(SSDbContext dbContext, ISessionTokenStorageService tokenStorageService)
        {
            _dbContext = dbContext;
            _tokenStorageService = tokenStorageService;
        }

        [HttpGet("iseedeadpeople")]
        public IActionResult DeleteAll()
        {
            return AutoView(new WipeItModel());
            //return AutoView();
        }

        [HttpPost("iseedeadpeople")]
        public async Task<IActionResult> DeleteAll(WipeItModel model)
        {
            if (model.PasswordOfDead != "greedisgood")
            {
                ModelState.AddModelError(nameof(model.PasswordOfDead), "Код смерти введён неверно");
            }

            if (ModelState.IsValid)
            {
                var users = await _dbContext.Set<UserEntity>().ToListAsync();
                _dbContext.RemoveRange(users);
                var teams = await _dbContext.Set<TeamEntity>().ToListAsync();
                _dbContext.RemoveRange(teams);
                var settigns = await _dbContext.Set<BindSettingsEntity>().ToListAsync();
                _dbContext.RemoveRange(settigns);
                var categories = await _dbContext.Set<ScoreCategoryEntity>().ToListAsync();
                _dbContext.RemoveRange(categories);
                await _dbContext.SaveChangesAsync();
                _tokenStorageService.RefreshToken();
                return Ok(new {status = "Красава, ты всё уничтожил!"});
            }
            return AutoView(model);
        }
    }
}