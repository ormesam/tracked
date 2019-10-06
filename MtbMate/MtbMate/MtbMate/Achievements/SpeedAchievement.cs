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
        public string AchievedText => IsAchieved ? "Achieved " + Time.Value.ToString("dd/MM/yy HH:mm") : "--";

        public SpeedAchievement(int id, double minimumMph) {
            Id = id;
            MinimumMph = minimumMph;
        }

        public bool Check(Ride ride) {
            if (IsAchieved) {
                return false;
            }

            return ride.Locations.Max(i => i.Mph) >= MinimumMph;
        }
    }
}
