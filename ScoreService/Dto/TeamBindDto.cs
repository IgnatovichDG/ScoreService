using ScoreService.Entities;

namespace ScoreService.Dto
{
    public class TeamBindDto
    {
        public TeamEntity Team { get; set; }
        public int Counter { get; set; } = 0;
    }
}