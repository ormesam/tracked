using System.Collections.Generic;
using MtbMate.Models;

namespace MtbMate.Achievements {
    public interface IAchievement {
        string Name { get; }
        int AchievedCount { get; }
        bool HasBeenAchieved { get; }
        string AchievedText { get; }

        bool Check(Ride ride);
        IEnumerable<Ride> GetRides();
    }
}
