namespace MtbMate.Contexts
{
    public class MainContext
    {
        public StorageContext Storage { get; }
        public ModelContext Model { get; set; }

        public MainContext()
        {
            Storage = new StorageContext();
            Model = new ModelContext(this);
        }
    }
}
