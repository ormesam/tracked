using System;
using Xamarin.Essentials;

namespace Tracked.Contexts {
    public class SettingsContext {
        public SettingsContext() {
        }

        public Guid? BluetoothDeviceId {
            get {
                if (Guid.TryParse(Preferences.Get("BluetoothDeviceId", null), out Guid guid)) {
                    return guid;
                }

                return null;
            }
            set { Preferences.Set("BluetoothDeviceId", value?.ToString()); }
        }

        public bool ShouldDetectJumps {
            get {
                if (bool.TryParse(Preferences.Get("ShouldDetectJumps", null), out bool boolValue)) {
                    return boolValue;
                }

                return false;
            }
            set { Preferences.Set("ShouldDetectJumps", value); }
        }
    }
}