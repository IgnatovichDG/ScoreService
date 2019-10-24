using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ScoreService.ViewModel
{
    public class ScoreModel
    {
        public string TeamName { get; set; }

        public long TeamId { get; set; }

        [BindProperty]
        public List<ScoreCategoryModel> Categories { get; set; }
    }
}