using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScoreService.Entities
{
    public class UserEntity
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public ZoneEntity Zone { get; set; }
       
        public ICollection<ScoreEntity> Scores { get; set; }

        public ICollection<UserTeamRelation> ScoredTeams { get; set; }

        internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
        {
            /// <inheritdoc />
            public void Configure(EntityTypeBuilder<UserEntity> builder)
            {
                builder
                    .HasOne(p => p.Zone)
                    .WithMany(p => p.Users)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}
