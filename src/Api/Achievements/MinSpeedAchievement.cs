using System.Linq;
using Shared.Dtos;

namespace Api.Achievements {
    public class MinSpeedAchievement : Achievement {
        public int SpeedAchievementId { get; set; }
        public decimal MinMph { get; set; }

        public override bool Check(RideDto ride) {
            return ride.Locations.Any(i => i.Mph >= MinMph);
        }
    }
}
