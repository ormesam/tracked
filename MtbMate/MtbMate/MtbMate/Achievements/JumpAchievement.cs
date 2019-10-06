using System;
using System.Linq;
using MtbMate.Models;

namespace MtbMate.Achievements {
    public class JumpAchievement : IAchievement {
        public int Id { get; }
        public string Name => $"Airtime {MinimumAirtime}s";
        public bool IsAchieved { get; set; }
        public double MinimumAirtime { get; set; }
        public DateTime? Time { get; set; }
        public Guid? RideId { get; set; }
        public string AchievedText => IsAchieved ? "Achieved " + Time.Value.ToString("dd/MM/yy HH:mm") : "--";

        public JumpAchievement(int id, double minimumAirtime) {
            Id = id;
            MinimumAirtime = minimumAirtime;
        }

        public bool Check(Ride ride) {
            if (IsAchieved) {
                return false;
            }

            if (!ride.Jumps.Any()) {
                return false;
            }

            return ride.Jumps.Max(i => i.Airtime) >= MinimumAirtime;
        }
    }
}
