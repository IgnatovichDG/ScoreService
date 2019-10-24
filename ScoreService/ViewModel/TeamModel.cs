using System.Collections.Generic;

namespace ScoreService.ViewModel
{
    public class TeamModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class CollectionTeamModel
    {
        public ICollection<TeamModel> Teams { get; set; }
    }
}