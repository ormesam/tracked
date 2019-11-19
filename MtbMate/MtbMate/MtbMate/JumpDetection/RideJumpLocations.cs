using System;
using System.Collections.Generic;
using System.Linq;
using MtbMate.Models;

namespace MtbMate.JumpDetection {
    public class RideJumpLocations : IJumpLocationDetector {
        private IList<Location> locations;

        public RideJumpLocations(IList<Location> locations) {
            this.locations = locations;
        }

        public Location GetLastLocation(DateTime time) {
            return locations
                .OrderBy(i => Math.Abs((i.Timestamp - time).TotalSeconds))
                .FirstOrDefault();
        }
    }
}
