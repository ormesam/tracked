using System;
using MtbMate.Models;

namespace MtbMate.JumpDetection {
    public interface IJumpLocationDetector {
        Location GetLastLocation(DateTime time);
    }
}
