using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScoreService.Entities
{
    public class UserTeamRelation
    {
        public long UserId { get; set; }

        public long TeamId { get; set; }

        public UserEntity User { get; set; }

        public TeamEntity Team { get; set; }

        public bool IsScored { get; set; }

        internal class UserTeamRelationTypeConfiguration : IEntityTypeConfiguration<UserTeamRelation>
        {
            
            /// <inheritdoc />
            public void Configure(EntityTypeBuilder<UserTeamRelation> builder)
            {
                builder.HasKey(p => new
                {
                    p.UserId, p.TeamId
                });

                builder.HasOne(p => p.User)
                    .WithMany(p=>p.ScoredTeams)
                    .HasForeignKey(p=>p.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(p=>p.Team)
                    .WithMany(p => p.Users)
                    .HasForeignKey(p=>p.TeamId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}