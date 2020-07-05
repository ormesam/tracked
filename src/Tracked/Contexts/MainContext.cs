using Tracked.Accelerometer;
using Tracked.Utilities;

namespace Tracked.Contexts {
    public class MainContext {
        public ModelContext Model { get; }
        public StorageContext Storage { get; }
        public UIContext UI { get; }
        public SecurityContext Security { get; }
        public ServicesContext Services { get; }
        public GeoUtility GeoUtility { get; set; }
        public AccelerometerUtility AccelerometerUtility { get; set; }
        public SettingsContext Settings { get; set; }

        public MainContext() {
            Storage = new StorageContext();
            Model = new ModelContext(this);
            UI = new UIContext(this);
            Security = new SecurityContext(this);
            Services = new ServicesContext(this);
            GeoUtility = new GeoUtility();
            AccelerometerUtility = new AccelerometerUtility();
            Settings = new SettingsContext();
        }
    }
}
