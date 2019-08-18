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

        public IList<SegmentModel> GetSegments()
        {
            var segments = Storage.GetAllObjects<SegmentModel>().Wait();

            return segments.ToList();
        }

        public async Task SaveObject<T>(Guid id, T obj)
        {
            await Storage.InsertObject(id.ToString(), obj);
        }

        public async Task RemoveObject<T>(Guid id)
        {
            await Storage.InvalidateObject<T>(id.ToString());
        }

        public SettingsModel GetSettings()
        {
            var settings = Storage.GetAllObjects<SettingsModel>().Wait().SingleOrDefault();

            if (settings == null)
            {
                settings = new SettingsModel();
                SaveSettings(settings).Wait();
            }

            return settings;
        }

        public async Task SaveSettings(SettingsModel settings)
        {
            if (settings.Id == null)
            {
                settings.Id = Guid.NewGuid();
                settings.ResetDefaults();
            }

            await SaveObject(settings.Id.Value, settings);
        }
    }
}
