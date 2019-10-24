using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreService.Entities;
using ScoreService.Infrastructure;
using ScoreService.ViewModel;

namespace ScoreService.Controllers
{
    [Route("iddqd")]
    public class WipeController : ControllerBase
    {
        private readonly SSDbContext _dbContext;

        public WipeController(SSDbContext dbContext)
        {
            _dbContext = dbContext;
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
                var categories = await _dbContext.Set<ScoreCategoryEntity>().ToListAsync();
                _dbContext.RemoveRange(categories);
                await _dbContext.SaveChangesAsync();
                return Ok(new {status = "Красава ты всё уничтожил!"});
            }
            return AutoView(model);
        }
    }
}