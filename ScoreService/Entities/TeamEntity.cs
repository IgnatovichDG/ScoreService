using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScoreService.Entities
{
    public class TeamEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public ICollection<UserTeamRelation> Users { get; set; }

        public ICollection<ScoreEntity> Score { get; set; }

        public ZoneEntity Zone { get; set; }
      

        internal class TeamEntityTypeConfiguration : IEntityTypeConfiguration<TeamEntity>
        {
            /// <inheritdoc />
            public void Configure(EntityTypeBuilder<TeamEntity> builder)
            {
                builder.HasOne(p => p.Zone)
                    .WithMany(p => p.Teams)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

           }
        }
    }
}