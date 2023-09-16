using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models {
    public class ModelDataContext : DbContext {
        public ModelDataContext() {
        }

        public ModelDataContext(DbContextOptions<ModelDataContext> options)
            : base(options) {
        }

        public DbSet<AccelerometerReading> AccelerometerReadings { get; set; }
        public DbSet<DistanceAchievement> DistanceAchievements { get; set; }
        public DbSet<Jump> Jumps { get; set; }
        public DbSet<JumpAchievement> JumpAchievements { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<RideLocation> RideLocations { get; set; }
        public DbSet<SpeedAchievement> SpeedAchievements { get; set; }
        public DbSet<TraceMessage> TraceMessages { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<TrailAttempt> TrailAttempts { get; set; }
        public DbSet<TrailLocation> TrailLocations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBlock> UserBlocks { get; set; }
        public DbSet<UserDistanceAchievement> UserDistanceAchievements { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<UserJumpAchievement> UserJumpAchievements { get; set; }
        public DbSet<UserSpeedAchievement> UserSpeedAchievements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer("Server=localhost;Database=TrackedDev;Trusted_Connection=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
