using System.Collections.Generic;
using System.Linq;

namespace Api.Analysers {
    public class MinSpeedAchievement {
        public string Name { get; set; }
        public int SpeedAchievementId { get; set; }
        public double MinMph { get; set; }

        internal bool Check(IEnumerable<RideLocationAnalysis> locations) {
            return locations.Any(i => i.Mph >= MinMph);
        }
    }
}
