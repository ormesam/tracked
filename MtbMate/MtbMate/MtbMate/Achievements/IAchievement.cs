using System;
using MtbMate.Models;

namespace MtbMate.Achievements {
    public interface IAchievement {
        int Id { get; }
        string Name { get; }
        bool IsAchieved { get; set; }
        DateTime? Time { get; set; }
        Guid? RideId { get; set; }

        bool Check(Ride ride);
    }
}
