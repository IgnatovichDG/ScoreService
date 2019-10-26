using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScoreService.Entities
{
    public class ZoneEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserEntity> Users { get; set; }

        public ICollection<TeamEntity> Teams { get; set; }

        internal class ZoneEntityTypeConfiguration : IEntityTypeConfiguration<ZoneEntity>
        {
            /// <inheritdoc />
            public void Configure(EntityTypeBuilder<ZoneEntity> builder)
            {
                builder.HasData(new List<ZoneEntity>()
                {
                    new ZoneEntity()
                    {
                        Id = 1,
                        Name = "A"
                    },
                    new ZoneEntity()
                    {
                        Id = 2,
                        Name = "B"
                    }
                });
            }
        }
    }
}