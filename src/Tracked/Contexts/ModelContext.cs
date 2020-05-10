using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Utilities;

namespace Tracked.Contexts {
    public class ModelContext {
        private readonly MainContext mainContext;
        private StorageContext Storage => mainContext.Storage;

        public ObservableCollection<CreateRideDto> PendingRideUploads { get; set; }

        public ModelContext(MainContext mainContext) {
            this.mainContext = mainContext;

            PendingRideUploads = Storage.GetPendingRideUploads().ToObservable();
        }

        public async Task SaveRideUpload(CreateRideDto ride) {
            if (ride.Id == null) {
                ride.Id = Guid.NewGuid();

                PendingRideUploads.Add(ride);
            }

            await Storage.SaveObject(ride.Id.Value, ride);
        }

        public async Task RemoveUploadRide(CreateRideDto ride) {
            PendingRideUploads.Remove(ride);

            await Storage.RemoveObject<CreateRideDto>(ride.Id.Value);
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
