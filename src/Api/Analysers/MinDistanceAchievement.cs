namespace Api.Analysers {
    public class MinDistanceAchievement {
        public int DistanceAchievementId { get; set; }
        public string Name { get; set; }
        public double MinDistanceMiles { get; set; }

        public bool Check(double distance) {
            return distance >= MinDistanceMiles;
        }
    }
}
