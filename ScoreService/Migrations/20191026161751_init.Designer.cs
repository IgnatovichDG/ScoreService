﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScoreService.Infrastructure;

namespace ScoreService.Migrations
{
    [DbContext(typeof(SSDbContext))]
    [Migration("20191026161751_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("ScoreService.Entities.BindSettingsEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MaxTeamForUserCounter");

                    b.Property<int>("MinUsersForTeamCounter");

                    b.HasKey("Id");

                    b.ToTable("BindSettingsEntity");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            MaxTeamForUserCounter = 15,
                            MinUsersForTeamCounter = 3
                        });
                });

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

                    b.Property<string>("Address");

                    b.Property<string>("Name");

                    b.Property<long>("ZoneId");

                    b.HasKey("Id");

                    b.HasIndex("ZoneId");

                    b.ToTable("TeamEntity");
                });

            modelBuilder.Entity("ScoreService.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Login");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordSalt");

                    b.Property<long>("ZoneId");

                    b.HasKey("Id");

                    b.HasIndex("ZoneId");

                    b.ToTable("UserEntity");
                });

            modelBuilder.Entity("ScoreService.Entities.UserTeamRelation", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("TeamId");

                    b.Property<bool>("IsScored");

                    b.HasKey("UserId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("UserTeamRelation");
                });

            modelBuilder.Entity("ScoreService.Entities.ZoneEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ZoneEntity");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "A"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "B"
                        });
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
                    b.HasOne("ScoreService.Entities.ZoneEntity", "Zone")
                        .WithMany("Teams")
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScoreService.Entities.UserEntity", b =>
                {
                    b.HasOne("ScoreService.Entities.ZoneEntity", "Zone")
                        .WithMany("Users")
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScoreService.Entities.UserTeamRelation", b =>
                {
                    b.HasOne("ScoreService.Entities.TeamEntity", "Team")
                        .WithMany("Users")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScoreService.Entities.UserEntity", "User")
                        .WithMany("ScoredTeams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
