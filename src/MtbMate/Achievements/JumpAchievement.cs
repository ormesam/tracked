using System.Linq;
using Tracked.Models;

namespace Tracked.Achievements {
    public class JumpAchievement : AchievementBase, IAchievement {
        public string Name => $"Airtime {MinimumAirtime}s";
        public double MinimumAirtime { get; set; }

        public JumpAchievement(double minimumAirtime) {
            MinimumAirtime = minimumAirtime;
        }

        public override bool Check(Ride ride) {
            if (!ride.Jumps.Any()) {
                return false;
            }

            return ride.Jumps.Max(i => i.Airtime) >= MinimumAirtime;
        }
    }
}
