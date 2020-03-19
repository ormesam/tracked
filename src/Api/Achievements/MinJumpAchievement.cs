using System.Linq;
using Shared.Dtos;

namespace Api.Achievements {
    public class MinJumpAchievement : Achievement {
        public int JumpAchievementId { get; set; }
        public decimal MinAirtime { get; set; }

        public override bool Check(RideDto ride) {
            return ride.Jumps.Any(i => i.Airtime >= MinAirtime);
        }
    }
}
