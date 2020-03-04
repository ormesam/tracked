using System;
using System.Collections.Generic;
using System.Linq;
using Tracked.Models;

namespace Tracked.Achievements {
    public abstract class AchievementBase {
        public abstract bool Check(Ride ride);

        public bool HasBeenAchieved => Model.Instance.Rides.Any(i => Check(i));

        public int AchievedCount => GetRides().Count();

        public string AchievedText {
            get {
                int achievedCount = AchievedCount;

                if (achievedCount == 0) {
                    return "--";
                }

                if (achievedCount == 1) {
                    return $"Achieved once on {GetLastRideDate()?.ToShortDateString()}";
                }

                return $"Achieved {AchievedCount} times, last on {GetLastRideDate()?.ToShortDateString()}";
            }
        }

        public IEnumerable<Ride> GetRides() {
            foreach (var ride in Model.Instance.Rides) {
                if (Check(ride)) {
                    yield return ride;
                }
            }
        }

        private DateTime? GetLastRideDate() {
            foreach (var ride in Model.Instance.Rides.OrderByDescending(i => i.Start)) {
                if (Check(ride)) {
                    return ride.Start;
                }
            }

            return null;
        }
    }
}
