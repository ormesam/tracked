using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Models
{
    public partial class ModelDataContext : DbContext
    {
        public ModelDataContext()
        {
        }

        public ModelDataContext(DbContextOptions<ModelDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccelerometerReading> AccelerometerReadings { get; set; }
        public virtual DbSet<DistanceAchievement> DistanceAchievements { get; set; }
        public virtual DbSet<Jump> Jumps { get; set; }
        public virtual DbSet<JumpAchievement> JumpAchievements { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
        public virtual DbSet<RideLocation> RideLocations { get; set; }
        public virtual DbSet<SpeedAchievement> SpeedAchievements { get; set; }
        public virtual DbSet<TraceMessage> TraceMessages { get; set; }
        public virtual DbSet<Trail> Trails { get; set; }
        public virtual DbSet<TrailAttempt> TrailAttempts { get; set; }
        public virtual DbSet<TrailLocation> TrailLocations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDistanceAchievement> UserDistanceAchievements { get; set; }
        public virtual DbSet<UserJumpAchievement> UserJumpAchievements { get; set; }
        public virtual DbSet<UserSpeedAchievement> UserSpeedAchievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<AccelerometerReading>(entity =>
            {
                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.AccelerometerReadings)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccelerometerReading_Ride");
            });

            modelBuilder.Entity<Jump>(entity =>
            {
                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.Jumps)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Jump_Ride");
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rides)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ride_User");
            });

            modelBuilder.Entity<RideLocation>(entity =>
            {
                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.RideLocations)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RideLocation_Ride");
            });

            modelBuilder.Entity<Trail>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Trails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trail_User");
            });

            modelBuilder.Entity<TrailAttempt>(entity =>
            {
                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.TrailAttempts)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrailAttempt_Ride");

                entity.HasOne(d => d.Trail)
                    .WithMany(p => p.TrailAttempts)
                    .HasForeignKey(d => d.TrailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrailAttempt_Trail");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TrailAttempts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrailAttempt_User");
            });

            modelBuilder.Entity<TrailLocation>(entity =>
            {
                entity.HasOne(d => d.Trail)
                    .WithMany(p => p.TrailLocations)
                    .HasForeignKey(d => d.TrailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrailLocation_Trail");
            });

            modelBuilder.Entity<UserDistanceAchievement>(entity =>
            {
                entity.HasOne(d => d.DistanceAchievement)
                    .WithMany(p => p.UserDistanceAchievements)
                    .HasForeignKey(d => d.DistanceAchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDistanceAchievement_DistanceAchievement");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.UserDistanceAchievements)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDistanceAchievement_Ride");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDistanceAchievements)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDistanceAchievement_User");
            });

            modelBuilder.Entity<UserJumpAchievement>(entity =>
            {
                entity.HasOne(d => d.JumpAchievement)
                    .WithMany(p => p.UserJumpAchievements)
                    .HasForeignKey(d => d.JumpAchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserJumpAchievement_JumpAchievement");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.UserJumpAchievements)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserJumpAchievement_Ride");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserJumpAchievements)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserJumpAchievement_User");
            });

            modelBuilder.Entity<UserSpeedAchievement>(entity =>
            {
                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.UserSpeedAchievements)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSpeedAchievement_Ride");

                entity.HasOne(d => d.SpeedAchievement)
                    .WithMany(p => p.UserSpeedAchievements)
                    .HasForeignKey(d => d.SpeedAchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSpeedAchievement_SpeedAchievement");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSpeedAchievements)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSpeedAchievement_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
