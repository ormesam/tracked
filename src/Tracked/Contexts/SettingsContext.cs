using Xamarin.Essentials;

namespace Tracked.Contexts {
    public class SettingsContext {
        public SettingsContext() {
        }

        public bool ShouldDetectJumps {
            get { return Preferences.Get("ShouldDetectJumps", false); }
            set { Preferences.Set("ShouldDetectJumps", value); }
        }
    }
}