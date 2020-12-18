using System;
using Xamarin.Forms.Maps;

namespace Tracked.Models {
    public class MapPin : Pin {
        public Guid PinId { get; set; }
        public float Rotation { get; set; }
        public bool IsSpeedPin { get; set; }
        public bool IsJumpPin { get; set; }

        public MapPin() {
            PinId = Guid.NewGuid();
        }
    }
}
