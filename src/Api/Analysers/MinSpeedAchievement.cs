using System.Linq;
using Shared.Dtos;

namespace Api.Analysers {
    public class MinSpeedAchievement {
        public string Name { get; set; }
        public int SpeedAchievementId { get; set; }
        public double MinMph { get; set; }

        internal bool Check(RideDto ride) {
            return ride.Locations.Any(i => i.Mph >= MinMph);
        }
    }
}
