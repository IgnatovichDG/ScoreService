using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScoreService.Entities
{
    public class ScoreEntity
    {
        public long Id { get; set; }

        public UserEntity User { get; set; }

        public TeamEntity Team { get; set; }

        public int Score { get; set; }

        public ScoreCategoryEntity Category { get; set; }

        internal class ScoreEntityTypeConfiguration : IEntityTypeConfiguration<ScoreEntity>
        {
            /// <inheritdoc />
            public void Configure(EntityTypeBuilder<ScoreEntity> builder)
            {
                builder.HasOne(p => p.User)
                    .WithMany(p => p.Scores)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne<ScoreCategoryEntity>()
                    .WithMany(p => p.Scores)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(p => p.Team)
                    .WithMany(p=>p.Score)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}