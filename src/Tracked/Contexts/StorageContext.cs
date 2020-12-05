using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Shared.Dtos;

namespace Tracked.Contexts {
    public class StorageContext {
        public readonly IBlobCache Storage = BlobCache.LocalMachine;

        public IList<CreateRideDto> GetPendingRideUploads() {
            var attempts = Storage.GetAllObjects<CreateRideDto>().Wait();

            return attempts.ToList();
        }

        public async Task SaveObject<T>(Guid id, T obj) {
            await Storage.InsertObject(id.ToString(), obj);
        }

        public async Task RemoveObject<T>(Guid id) {
            await Storage.InvalidateObject<T>(id.ToString());
        }
    }
}
