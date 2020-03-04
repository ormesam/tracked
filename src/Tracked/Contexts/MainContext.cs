using Tracked.Models;

namespace Tracked.Contexts {
    public class MainContext {
        public StorageContext Storage { get; }
        public UIContext UI { get; }
        public SecurityContext Security { get; }
        public ServicesContext Services { get; }
        public Settings Settings { get; }

        public MainContext() {
            Storage = new StorageContext();
            UI = new UIContext(this);
            Security = new SecurityContext(this);
            Services = new ServicesContext(this);
            Settings = Storage.GetSettings();
        }
    }
}
