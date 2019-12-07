using System.Collections.Generic;
using System.Linq;
using MtbMate.Models;

namespace MtbMate.Achievements {
    public abstract class AchievementBase {
        public abstract bool Check(Ride ride);

        public bool HasBeenAchieved => GetRides().Any();

        public int AchievedCount => GetRides().Count();

        public string AchievedText {
            get {
                if (HasBeenAchieved) {
                    return $"Achieved {AchievedCount} times";
                }

                return "--";
            }
        }

        public IEnumerable<Ride> GetRides() {
            foreach (var ride in Model.Instance.Rides) {
                if (Check(ride)) {
                    yield return ride;
                }
            }
        }
    }
}
