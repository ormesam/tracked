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
        public virtual DbSet<Ride> Ride { get; set; }
        public virtual DbSet<RideJump> RideJump { get; set; }
        public virtual DbSet<RideLocation> RideLocation { get; set; }
        public virtual DbSet<Segment> Segment { get; set; }
        public virtual DbSet<SegmentAttempt> SegmentAttempt { get; set; }
        public virtual DbSet<SegmentAttemptJump> SegmentAttemptJump { get; set; }
        public virtual DbSet<SegmentAttemptLocation> SegmentAttemptLocation { get; set; }
        public virtual DbSet<SegmentLocation> SegmentLocation { get; set; }
        public virtual DbSet<TraceMessage> TraceMessage { get; set; }
        public virtual DbSet<User> User { get; set; }

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

                entity.Property(e => e.X).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.Y).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.Z).HasColumnType("decimal(5, 3)");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.AccelerometerReading)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccelerometerReading_Ride");
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.Property(e => e.AverageSpeedMph).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.DistanceMiles).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.EndUtc).HasColumnType("datetime");

                entity.Property(e => e.MaxSpeedMph).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.StartUtc).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ride)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ride_User");
            });

            modelBuilder.Entity<RideJump>(entity =>
            {
                entity.Property(e => e.Airtime).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.RideJump)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RideJump_Ride");
            });

            modelBuilder.Entity<RideLocation>(entity =>
            {
                entity.Property(e => e.AccuracyInMetres).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.Altitude).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.Latitude).HasColumnType("decimal(25, 20)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(25, 20)");

                entity.Property(e => e.SpeedMetresPerSecond).HasColumnType("decimal(6, 3)");

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
                entity.Property(e => e.AverageSpeedMph).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.DistanceMiles).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.EndUtc).HasColumnType("datetime");

                entity.Property(e => e.MaxSpeedMph).HasColumnType("decimal(4, 1)");

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
                entity.Property(e => e.Airtime).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.SegmentAttempt)
                    .WithMany(p => p.SegmentAttemptJump)
                    .HasForeignKey(d => d.SegmentAttemptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentAttemptJump_SegmentAttempt");
            });

            modelBuilder.Entity<SegmentAttemptLocation>(entity =>
            {
                entity.Property(e => e.AccuracyInMetres).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.Altitude).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.Latitude).HasColumnType("decimal(25, 20)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(25, 20)");

                entity.Property(e => e.SpeedMetresPerSecond).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.SegmentAttempt)
                    .WithMany(p => p.SegmentAttemptLocation)
                    .HasForeignKey(d => d.SegmentAttemptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentAttemptLocation_SegmentAttempt");
            });

            modelBuilder.Entity<SegmentLocation>(entity =>
            {
                entity.Property(e => e.Latitude).HasColumnType("decimal(25, 20)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(25, 20)");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SegmentLocation)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentLocation_Segment");
            });

            modelBuilder.Entity<TraceMessage>(entity =>
            {
                entity.Property(e => e.DateUtc).HasColumnType("datetime");

                entity.Property(e => e.Message).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.GoogleUserId)
                    .HasName("UQ__User__437CD1971456557A")
                    .IsUnique();

                entity.Property(e => e.GoogleUserId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
