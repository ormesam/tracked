using System;
using System.Collections.Generic;

namespace Tracked.Models {
    public interface IRide {
        Guid? Id { get; }
        string DisplayName { get; }
        DateTime Start { get; }
        DateTime End { get; }
        IList<Location> Locations { get; }
        IList<Jump> Jumps { get; }
        IList<AccelerometerReading> AccelerometerReadings { get; }
        bool ShowAttempts { get; }
    }
}
