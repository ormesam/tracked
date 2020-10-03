using Shared.Dtos;

namespace Api.Analysers {
    public class MinDistanceAchievement {
        public int DistanceAchievementId { get; set; }
        public string Name { get; set; }
        public double MinDistanceMiles { get; set; }

        internal bool Check(RideDto ride) {
            return ride.DistanceMiles >= MinDistanceMiles;
        }
    }
}
