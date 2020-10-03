using System.Linq;
using Shared.Dtos;

namespace Api.Analysers {
    public class MinJumpAchievement {
        public int JumpAchievementId { get; set; }
        public string Name { get; set; }
        public double MinAirtime { get; set; }

        internal bool Check(RideDto ride) {
            return ride.Jumps.Any(i => i.Airtime >= MinAirtime);
        }
    }
}
