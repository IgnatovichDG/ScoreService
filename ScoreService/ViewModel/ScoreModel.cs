using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreService.Entities;

namespace ScoreService.ViewModel
{
    public class ScoreModel
    {
        public string TeamName { get; set; }

        public long TeamId { get; set; }

        [BindProperty]
        public List<ScoreCategoryModel> Categories { get; set; }
    }

    public class ScoreCategoryModel
    {
        public long Id { get; set; }

        public ScoreCategoryKind CategoryKind { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}