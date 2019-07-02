using System.Collections.Generic;
using System.Threading.Tasks;
using MtbMate.Models;

namespace MtbMate.Contexts
{
    public class ModelContext
    {
        private StorageContext storage;

        public IList<RideModel> Rides { get; set; }

        public ModelContext(MainContext mainContext)
        {
            storage = mainContext.Storage;
            Rides = mainContext.Storage.GetRides();
        }

        public async Task AddRide(RideModel ride)
        {
            Rides.Add(ride);
            await storage.AddObject(ride);
        }
    }
}
