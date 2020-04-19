using Xamarin.Forms.Maps;

namespace Tracked.Controls {
    public class CustomMapPin : Pin {
        public float Rotation { get; set; }
        public bool IsSpeedPin { get; set; }
        public bool IsJumpPin { get; set; }
    }
}
