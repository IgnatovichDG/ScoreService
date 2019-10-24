using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreService.Entities;
using ScoreService.Infrastructure;
using ScoreService.ViewModel;

namespace ScoreService.Services
{
    class TeamService : ITeamService
    {
        private readonly SSDbContext _context;

        public TeamService(SSDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsScoredAsync(long teamId)
        {
            var team = await _context.Set<TeamEntity>().FirstOrDefaultAsync(p => p.Id == teamId);
            if (team == null)
                throw new ArgumentException();
            return team.IsScored;
        }

        public async Task<ICollection<ScoreCategoryModel>> GetCategoriesAsync()
        {
            var categories = await _context.Set<ScoreCategoryEntity>().Select(p => new ScoreCategoryModel()
            {
                Id = p.Id,
                Name = p.Name,
                Value = null,
                CategoryKind = p.Kind
            }).ToListAsync();
            return categories;
        }

        public async Task SaveScoreAsync(ScoreModel model, string login)
        {
            var user = await _context.Set<UserEntity>().FirstAsync(p => p.Login == login);
            var team = await _context.Set<TeamEntity>().FirstAsync(p => p.Id == model.TeamId);
            foreach (var category in model.Categories)
            {
                var categoryEntity = await _context.Set<ScoreCategoryEntity>().FirstAsync(p => p.Id == category.Id);
                
                await _context.AddAsync(new ScoreEntity()
                {
                    Score = category.Value,
                    Category = categoryEntity,
                    User = user,
                    Team = team
                });
            }

            await _context.SaveChangesAsync();
            team.IsScored = true;
            _context.Update(team);
            await _context.SaveChangesAsync();
        }
    }
}