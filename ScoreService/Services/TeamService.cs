using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreService.Dto;
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

        public async Task<bool> IsScoredAsync(string login, long teamId)
        {
            var user = await _context.Set<UserEntity>().FirstOrDefaultAsync(p => p.Login == login);
            var team = await _context.Set<TeamEntity>().FirstOrDefaultAsync(p => p.Id == teamId);
            if (team == null || user == null)
                throw new ArgumentException();
            var userTeamRelation =
                _context.Set<UserTeamRelation>().First(p => p.UserId == user.Id && p.TeamId == team.Id);
            return userTeamRelation.IsScored;
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
            var userTeamRelation =
                _context.Set<UserTeamRelation>().First(p => p.UserId == user.Id && p.TeamId == team.Id);
            userTeamRelation.IsScored = true;
            _context.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task BindTeamsToUser()
        {
            var zones = await _context.Set<ZoneEntity>().Include(p => p.Teams).Include(p => p.Users).ToListAsync();
            var settings = await _context.Set<BindSettingsEntity>().FirstAsync();

            foreach (var zone in zones)
            {

                var users = zone.Users.ToList();
                var teams = zone.Teams.Select(p => new TeamBindDto
                {
                    Team = p,
                    Counter = 0
                }).ToList();

                foreach (var user in users)
                {
                    while (user.ScoredTeams.Count < settings.MaxTeamForUserCounter)
                    {
                        var teamsForUser = teams.Where(p => p.Counter < settings.MinUsersForTeamCounter).ToList();
                        if (!teamsForUser.Any())
                        {
                            teamsForUser = teams.OrderBy(p => p.Counter).ToList();
                        }

                        TeamBindDto teamDto = null;
                        foreach (var team in teamsForUser)
                        {
                            if (user.ScoredTeams.All(p => p.Team.Id != team.Team.Id))
                            {
                                teamDto = team;
                                break;
                            }
                        }
                        if(teamDto == null)
                            break;
                        user.ScoredTeams.Add(new UserTeamRelation()
                        {
                            User = user,
                            Team = teamDto.Team
                        });
                        teamDto.Counter++;

                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveBindings()
        {
            var users = await _context.Set<UserEntity>().Include(p => p.ScoredTeams).ToListAsync();
            foreach (var user in users)
            {
                user.ScoredTeams = new List<UserTeamRelation>();
                _context.Update(user);
            }

            await _context.SaveChangesAsync();
        }
    }
}