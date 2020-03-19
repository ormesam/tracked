using System;
using Shared.Dtos;

namespace Tracked.JumpDetection {
    public interface IJumpLocationDetector {
        RideLocationDto GetLastLocation(DateTime time);
    }
}
