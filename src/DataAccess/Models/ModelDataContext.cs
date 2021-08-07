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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=TrackedDev;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<AccelerometerReading>(entity =>
            {
                entity.ToTable("AccelerometerReading");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.AccelerometerReadings)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccelerometerReading_Ride");
            });

            modelBuilder.Entity<DistanceAchievement>(entity =>
            {
                entity.ToTable("DistanceAchievement");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Jump>(entity =>
            {
                entity.ToTable("Jump");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.Jumps)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Jump_Ride");
            });

            modelBuilder.Entity<JumpAchievement>(entity =>
            {
                entity.ToTable("JumpAchievement");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.ToTable("Ride");

                entity.Property(e => e.EndUtc).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.RouteSvgPath).IsRequired();

                entity.Property(e => e.StartUtc).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rides)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ride_User");
            });

            modelBuilder.Entity<RideLocation>(entity =>
            {
                entity.ToTable("RideLocation");

                entity.Property(e => e.RemovalReason).HasMaxLength(255);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.RideLocations)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RideLocation_Ride");
            });

            modelBuilder.Entity<SpeedAchievement>(entity =>
            {
                entity.ToTable("SpeedAchievement");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<TraceMessage>(entity =>
            {
                entity.ToTable("TraceMessage");

                entity.Property(e => e.DateUtc).HasColumnType("datetime");

                entity.Property(e => e.Message).IsRequired();
            });

            modelBuilder.Entity<Trail>(entity =>
            {
                entity.ToTable("Trail");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Trails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trail_User");
            });

            modelBuilder.Entity<TrailAttempt>(entity =>
            {
                entity.ToTable("TrailAttempt");

                entity.Property(e => e.EndUtc).HasColumnType("datetime");

                entity.Property(e => e.StartUtc).HasColumnType("datetime");

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
                entity.ToTable("TrailLocation");

                entity.HasOne(d => d.Trail)
                    .WithMany(p => p.TrailLocations)
                    .HasForeignKey(d => d.TrailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrailLocation_Trail");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.GoogleUserId, "UQ__User__437CD1974104BF57")
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

                entity.Property(e => e.RefreshToken)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UserDistanceAchievement>(entity =>
            {
                entity.ToTable("UserDistanceAchievement");

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
                entity.ToTable("UserJumpAchievement");

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
                entity.ToTable("UserSpeedAchievement");

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
