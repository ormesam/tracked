using System;
using System.Linq;
using MtbMate.Models;

namespace MtbMate.Achievements {
    public class SpeedAchievement : IAchievement {
        public int Id { get; }
        public string Name => $"Exceeded {MinimumMph} mi/h";
        public bool IsAchieved { get; set; }
        public double MinimumMph { get; set; }
        public DateTime? Time { get; set; }
        public Guid? RideId { get; set; }

        public SpeedAchievement(int id, double minimumMph) {
            Id = id;
            MinimumMph = minimumMph;
        }

        public bool Check(Ride ride) {
            return ride.Locations.Max(i => i.Mph) >= MinimumMph;
        }
    }
}
