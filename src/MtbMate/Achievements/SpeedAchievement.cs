using System.Linq;
using Tracked.Models;

namespace Tracked.Achievements {
    public class SpeedAchievement : AchievementBase, IAchievement {
        public string Name => $"Exceeded {MinimumMph} mi/h";
        public double MinimumMph { get; set; }

        public SpeedAchievement(double minimumMph) {
            MinimumMph = minimumMph;
        }

        public override bool Check(Ride ride) {
            return ride.Locations.Max(i => i.Mph) >= MinimumMph;
        }
    }
}
