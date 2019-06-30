using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using MtbMate.Models;

namespace MtbMate.Contexts
{
    public class StorageContext
    {
        public readonly ISecureBlobCache Storage = BlobCache.InMemory;

        public IList<RideModel> LoadRides()
        {
            var rides = Storage.GetAllObjects<RideModel>().Wait();

            return rides.ToList();
        }

        public async Task AddRide(RideModel ride)
        {
            await Storage.InsertObject(Guid.NewGuid().ToString(), ride);
        }
    }
}
