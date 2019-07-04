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
        public readonly IBlobCache Storage = BlobCache.LocalMachine;

        public IList<RideModel> GetRides()
        {
            var rides = Storage.GetAllObjects<RideModel>().Wait();

            return rides.ToList();
        }

        public async Task AddObject<T>(Guid id, T obj)
        {
            await Storage.InsertObject(id.ToString(), obj);
        }

        public async Task RemoveObject<T>(Guid id)
        {
            await Storage.InvalidateObject<T>(id.ToString());
        }
    }
}
