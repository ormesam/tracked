using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Utilities;

namespace Tracked.Models {
    public class Model {
        #region Singleton stuff

        private static Model instance;
        private static readonly object _lock = new object();

        public static Model Instance {
            get {
                lock (_lock) {
                    if (instance == null) {
                        instance = new Model();
                    }

                    return instance;
                }
            }
        }

        #endregion

        private StorageContext storage;

        public ObservableCollection<RideUploadDto> PendingRideUploads { get; set; }

        private Model() {
        }

        public void Init(MainContext mainContext) {
            storage = mainContext.Storage;

            PendingRideUploads = storage.GetPendingRideUploads().ToObservable();
        }

        public async Task SaveRideUpload(RideUploadDto ride) {
            if (ride.Id == null) {
                ride.Id = Guid.NewGuid();

                PendingRideUploads.Add(ride);
            }

            await storage.SaveObject(ride.Id.Value, ride);
        }

        public async Task RemoveUploadRide(RideUploadDto ride) {
            PendingRideUploads.Remove(ride);

            await storage.RemoveObject<RideUploadDto>(ride.Id.Value);
        }

#if DEBUG
        public async Task RunUtilityAsync() {
            // Perform single use operations here such as fixing data.

            Toast.LongAlert("Done!");

            await Task.CompletedTask;
        }
#endif
    }
}
