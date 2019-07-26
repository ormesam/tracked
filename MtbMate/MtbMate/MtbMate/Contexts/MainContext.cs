using MtbMate.Models;

namespace MtbMate.Contexts
{
    public class MainContext
    {
        public StorageContext Storage { get; }
        public ModelContext Model { get; }
        public UIContext UI { get; }
        public SettingsModel Settings { get; }

        public MainContext()
        {
            Storage = new StorageContext();
            Model = new ModelContext(this);
            UI = new UIContext(this);
            Settings = Storage.GetSettings();
        }
    }
}
