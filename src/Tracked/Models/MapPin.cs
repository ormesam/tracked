using Xamarin.Forms.Maps;

namespace Tracked.Models {
    public class MapPin : Pin {
        public float Rotation { get; set; }
        public bool IsSpeedPin { get; set; }
        public bool IsJumpPin { get; set; }
    }
}
