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
        public virtual DbSet<Jump> Jump { get; set; }
        public virtual DbSet<JumpAchievement> JumpAchievement { get; set; }
        public virtual DbSet<Ride> Ride { get; set; }
        public virtual DbSet<RideLocation> RideLocation { get; set; }
        public virtual DbSet<Segment> Segment { get; set; }
        public virtual DbSet<SegmentAttempt> SegmentAttempt { get; set; }
        public virtual DbSet<SegmentAttemptJump> SegmentAttemptJump { get; set; }
        public virtual DbSet<SegmentAttemptLocation> SegmentAttemptLocation { get; set; }
        public virtual DbSet<SegmentLocation> SegmentLocation { get; set; }
        public virtual DbSet<SpeedAchievement> SpeedAchievement { get; set; }
        public virtual DbSet<TraceMessage> TraceMessage { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserJumpAchievement> UserJumpAchievement { get; set; }
        public virtual DbSet<UserSpeedAchievement> UserSpeedAchievement { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=TrackedDev;Trusted_Connection=True;");
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

            modelBuilder.Entity<Segment>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Segment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Segment_User");
            });

            modelBuilder.Entity<SegmentAttempt>(entity =>
            {
                entity.Property(e => e.EndUtc).HasColumnType("datetime");

                entity.Property(e => e.StartUtc).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.SegmentAttempt)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentAttempt_Ride");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SegmentAttempt)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentAttempt_Segment");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SegmentAttempt)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentAttempt_User");
            });

            modelBuilder.Entity<SegmentAttemptJump>(entity =>
            {
                entity.HasOne(d => d.Jump)
                    .WithMany(p => p.SegmentAttemptJump)
                    .HasForeignKey(d => d.JumpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentAttemptJump_Jump");

                entity.HasOne(d => d.SegmentAttempt)
                    .WithMany(p => p.SegmentAttemptJump)
                    .HasForeignKey(d => d.SegmentAttemptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentAttemptJump_SegmentAttempt");
            });

            modelBuilder.Entity<SegmentAttemptLocation>(entity =>
            {
                entity.HasOne(d => d.RideLocation)
                    .WithMany(p => p.SegmentAttemptLocation)
                    .HasForeignKey(d => d.RideLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentAttemptLocation_RideLocation");

                entity.HasOne(d => d.SegmentAttempt)
                    .WithMany(p => p.SegmentAttemptLocation)
                    .HasForeignKey(d => d.SegmentAttemptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentAttemptLocation_SegmentAttempt");
            });

            modelBuilder.Entity<SegmentLocation>(entity =>
            {
                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SegmentLocation)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentLocation_Segment");
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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.GoogleUserId)
                    .HasName("UQ__User__437CD197474A84E7")
                    .IsUnique();

                entity.Property(e => e.GoogleUserId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);
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
