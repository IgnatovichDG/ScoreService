using ScoreService.Entities;

namespace ScoreService.Dto
{
    public class ScoreCategoryDto
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public ScoreCategoryKind Kind { get; set; }

        public bool IsDeleted { get; set; }
    }
}