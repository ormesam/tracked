using System.Collections.Generic;
using System.Linq;

namespace Api.Analysers {
    public class MinSpeedAchievement {
        public string Name { get; set; }
        public int SpeedAchievementId { get; set; }
        public double MinMph { get; set; }

        public bool Check(IList<double> speeds) {
            return speeds.Any(i => i >= MinMph);
        }
    }
}
