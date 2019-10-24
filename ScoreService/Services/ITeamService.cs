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
    public interface ITeamService
    {
        Task<bool> IsScoredAsync(long teamId);
        Task<ICollection<ScoreCategoryModel>> GetCategoriesAsync();
    }

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
    }
}