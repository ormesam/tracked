using System;
using Tracked.Models;

namespace Tracked.JumpDetection {
    public interface IJumpLocationDetector {
        Location GetLastLocation(DateTime time);
    }
}
