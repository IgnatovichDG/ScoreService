using ScoreService.Entities;

namespace ScoreService.ViewModel
{
    public class ScoreCategoryModel
    {
        public long Id { get; set; }

        public ScoreCategoryKind CategoryKind { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}