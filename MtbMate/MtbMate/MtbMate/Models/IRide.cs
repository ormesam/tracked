using System;
using System.Collections.Generic;
using MtbMate.Contexts;

namespace MtbMate.Models {
    public interface IRide {
        Guid? Id { get; }
        string DisplayName { get; }
        DateTime? Start { get; }
        DateTime? End { get; }
        IList<Location> Locations { get; }
        IList<Jump> Jumps { get; }
        IList<AccelerometerReading> AccelerometerReadings { get; }
        void ChangeName(UIContext ui, Action whenCompete);
        bool CanChangeName { get; }
        bool ShowAttempts { get; }
    }
}
