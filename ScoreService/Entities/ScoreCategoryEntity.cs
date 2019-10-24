using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScoreService.Entities
{
    public class ScoreCategoryEntity
    {
        public long Id { get; set; }

        public string Alias { get; set; }

        public string Name { get; set; }

        public ScoreCategoryKind Kind { get; set; }

        public ICollection<ScoreEntity> Scores { get; set; }

        internal class ScoreCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ScoreCategoryEntity>
        {
            /// <inheritdoc />
            public void Configure(EntityTypeBuilder<ScoreCategoryEntity> builder)
            {
            }
        }
    }

    public enum ScoreCategoryKind
    {
        Number,
        String
    }
}