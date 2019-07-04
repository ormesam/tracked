using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MtbMate.Models;
using MtbMate.Utilities;

namespace MtbMate.Contexts
{
    public class ModelContext
    {
        private StorageContext storage;

        public ObservableCollection<RideModel> Rides { get; set; }

        public ModelContext(MainContext mainContext)
        {
            storage = mainContext.Storage;
            Rides = mainContext.Storage.GetRides().ToObservable();
        }

        public async Task AddRide(RideModel ride)
        {
            Rides.Add(ride);
            await storage.AddObject(ride);
        }
    }
}
