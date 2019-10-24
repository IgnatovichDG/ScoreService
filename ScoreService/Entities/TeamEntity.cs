using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScoreService.Entities
{
    public class TeamEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public UserEntity User { get; set; }

        public ICollection<ScoreEntity> Score { get; set; }

        public bool IsScored { get; set; }

        internal class TeamEntityTypeConfiguration : IEntityTypeConfiguration<TeamEntity>
        {
            /// <inheritdoc />
            public void Configure(EntityTypeBuilder<TeamEntity> builder)
            {
                builder.HasOne(p => p.User)
                    .WithMany(p => p.ScoredTeams)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}