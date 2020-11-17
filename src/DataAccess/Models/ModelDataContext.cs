using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public virtual DbSet<AccelerometerReading> AccelerometerReading { get; set; }
        public virtual DbSet<DistanceAchievement> DistanceAchievement { get; set; }
        public virtual DbSet<Jump> Jump { get; set; }
        public virtual DbSet<JumpAchievement> JumpAchievement { get; set; }
        public virtual DbSet<Ride> Ride { get; set; }
        public virtual DbSet<RideLocation> RideLocation { get; set; }
        public virtual DbSet<SpeedAchievement> SpeedAchievement { get; set; }
        public virtual DbSet<TraceMessage> TraceMessage { get; set; }
        public virtual DbSet<Trail> Trail { get; set; }
        public virtual DbSet<TrailAttempt> TrailAttempt { get; set; }
        public virtual DbSet<TrailLocation> TrailLocation { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserDistanceAchievement> UserDistanceAchievement { get; set; }
        public virtual DbSet<UserJumpAchievement> UserJumpAchievement { get; set; }
        public virtual DbSet<UserSpeedAchievement> UserSpeedAchievement { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=TrackedDev;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccelerometerReading>(entity =>
            {
                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.AccelerometerReading)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccelerometerReading_Ride");
            });

            modelBuilder.Entity<DistanceAchievement>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Jump>(entity =>
            {
                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.Jump)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Jump_Ride");
            });

            modelBuilder.Entity<JumpAchievement>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.Property(e => e.EndUtc).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.RouteSvgPath).IsRequired();

                entity.Property(e => e.StartUtc).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ride)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ride_User");
            });

            modelBuilder.Entity<RideLocation>(entity =>
            {
                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.RideLocation)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RideLocation_Ride");
            });

            modelBuilder.Entity<SpeedAchievement>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<TraceMessage>(entity =>
            {
                entity.Property(e => e.DateUtc).HasColumnType("datetime");

                entity.Property(e => e.Message).IsRequired();
            });

            modelBuilder.Entity<Trail>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Trail)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trail_User");
            });

            modelBuilder.Entity<TrailAttempt>(entity =>
            {
                entity.Property(e => e.EndUtc).HasColumnType("datetime");

                entity.Property(e => e.StartUtc).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.TrailAttempt)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrailAttempt_Ride");

                entity.HasOne(d => d.Trail)
                    .WithMany(p => p.TrailAttempt)
                    .HasForeignKey(d => d.TrailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrailAttempt_Trail");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TrailAttempt)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrailAttempt_User");
            });

            modelBuilder.Entity<TrailLocation>(entity =>
            {
                entity.HasOne(d => d.Trail)
                    .WithMany(p => p.TrailLocation)
                    .HasForeignKey(d => d.TrailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrailLocation_Trail");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.GoogleUserId)
                    .HasName("UQ__User__437CD197B5AD85E5")
                    .IsUnique();

                entity.Property(e => e.CreatedUtc).HasColumnType("datetime");

                entity.Property(e => e.GoogleUserId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ProfileImageUrl)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UserDistanceAchievement>(entity =>
            {
                entity.HasOne(d => d.DistanceAchievement)
                    .WithMany(p => p.UserDistanceAchievement)
                    .HasForeignKey(d => d.DistanceAchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDistanceAchievement_DistanceAchievement");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.UserDistanceAchievement)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDistanceAchievement_Ride");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDistanceAchievement)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDistanceAchievement_User");
            });

            modelBuilder.Entity<UserJumpAchievement>(entity =>
            {
                entity.HasOne(d => d.JumpAchievement)
                    .WithMany(p => p.UserJumpAchievement)
                    .HasForeignKey(d => d.JumpAchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserJumpAchievement_JumpAchievement");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.UserJumpAchievement)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserJumpAchievement_Ride");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserJumpAchievement)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserJumpAchievement_User");
            });

            modelBuilder.Entity<UserSpeedAchievement>(entity =>
            {
                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.UserSpeedAchievement)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSpeedAchievement_Ride");

                entity.HasOne(d => d.SpeedAchievement)
                    .WithMany(p => p.UserSpeedAchievement)
                    .HasForeignKey(d => d.SpeedAchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSpeedAchievement_SpeedAchievement");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSpeedAchievement)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSpeedAchievement_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
