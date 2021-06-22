using System.Collections.Generic;
using System.Linq;

namespace Api.Analysers {
    public class MinJumpAchievement {
        public int JumpAchievementId { get; set; }
        public string Name { get; set; }
        public double MinAirtime { get; set; }

        public bool Check(IList<double> airtimes) {
            return airtimes.Any(i => i >= MinAirtime);
        }
    }
}
