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

        public JumpAchievement(int id, double minimumAirtime) {
            Id = id;
            MinimumAirtime = minimumAirtime;
        }

        public bool Check(Ride ride) {
            return ride.Jumps.Max(i => i.Airtime) >= MinimumAirtime;
        }
    }
}
