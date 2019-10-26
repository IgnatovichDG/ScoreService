using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScoreService.Entities
{
    public class BindSettingsEntity
    {
        public long Id { get; set; }

        public int MaxTeamForUserCounter { get; set; }

        public int MinUsersForTeamCounter { get; set; }

        internal class BindSettingsEntityTypeConfiguration : IEntityTypeConfiguration<BindSettingsEntity>
        {
            /// <inheritdoc />
            public void Configure(EntityTypeBuilder<BindSettingsEntity> builder)
            {
                builder.HasData(new List<BindSettingsEntity>()
                {
                    new BindSettingsEntity()
                    {
                        Id = 1,
                        MaxTeamForUserCounter = 15,
                        MinUsersForTeamCounter = 3
                    }
                });
            }
        }
    }
}