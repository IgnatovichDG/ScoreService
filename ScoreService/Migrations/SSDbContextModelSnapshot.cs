﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScoreService.Infrastructure;

namespace ScoreService.Migrations
{
    [DbContext(typeof(SSDbContext))]
    partial class SSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("ScoreService.Entities.ScoreCategoryEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<int>("Kind");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ScoreCategoryEntity");
                });

            modelBuilder.Entity("ScoreService.Entities.ScoreEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CategoryId");

                    b.Property<string>("Score");

                    b.Property<long>("TeamId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("ScoreEntity");
                });

            modelBuilder.Entity("ScoreService.Entities.TeamEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsScored");

                    b.Property<string>("Name");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TeamEntity");
                });

            modelBuilder.Entity("ScoreService.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Login");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordSalt");

                    b.HasKey("Id");

                    b.ToTable("UserEntity");
                });

            modelBuilder.Entity("ScoreService.Entities.ScoreEntity", b =>
                {
                    b.HasOne("ScoreService.Entities.ScoreCategoryEntity", "Category")
                        .WithMany("Scores")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScoreService.Entities.TeamEntity", "Team")
                        .WithMany("Score")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScoreService.Entities.UserEntity", "User")
                        .WithMany("Scores")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScoreService.Entities.TeamEntity", b =>
                {
                    b.HasOne("ScoreService.Entities.UserEntity", "User")
                        .WithMany("ScoredTeams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
