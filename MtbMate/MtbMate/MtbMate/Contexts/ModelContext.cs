using System;
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
            ride.Id = Guid.NewGuid();

            Rides.Add(ride);

            await storage.AddObject(ride.Id.Value, ride);
        }

        public async Task RemoveRide(RideModel ride)
        {
            Rides.Remove(ride);

            await storage.RemoveObject<RideModel>(ride.Id.Value);
        }
    }
}
