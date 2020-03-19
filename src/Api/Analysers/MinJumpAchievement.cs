using System.Collections.Generic;
using System.Linq;

namespace Api.Analysers {
    public class MinJumpAchievement {
        public int JumpAchievementId { get; set; }
        public string Name { get; set; }
        public decimal MinAirtime { get; set; }

        internal bool Check(IEnumerable<RideJumpAnalysis> jumps) {
            return jumps.Any(i => i.Airtime >= MinAirtime);
        }
    }
}
