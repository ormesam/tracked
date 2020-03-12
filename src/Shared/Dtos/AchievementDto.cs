using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Dtos {
    public class AchievementDto {
        public int AchievementId { get; set; }
        public string Name { get; set; }
        public IList<RideOverviewDto> Rides { get; set; }

        public bool HasBeenAchieved => Rides.Any();
        public int AchievedCount => Rides.Count();
        public DateTime? LastAchieved => Rides.Max(i => (DateTime?)i.StartUtc);

        public string AchievedText {
            get {
                int achievedCount = AchievedCount;

                if (achievedCount == 0) {
                    return "--";
                }

                if (achievedCount == 1) {
                    return $"Achieved once on {LastAchieved?.ToShortDateString()}";
                }

                return $"Achieved {AchievedCount} times, last on {LastAchieved?.ToShortDateString()}";
            }
        }

        public AchievementDto() {
            Rides = new List<RideOverviewDto>();
        }
    }
}
