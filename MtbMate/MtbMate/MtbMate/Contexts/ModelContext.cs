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
        public ObservableCollection<SegmentModel> Segments { get; set; }

        public ModelContext(MainContext mainContext)
        {
            storage = mainContext.Storage;
            Rides = mainContext.Storage.GetRides().ToObservable();
            Segments = mainContext.Storage.GetSegments().ToObservable();
        }

        public async Task SaveRide(RideModel ride)
        {
            if (ride.Id == null)
            {
                ride.Id = Guid.NewGuid();

                Rides.Add(ride);
            }

            await storage.SaveObject(ride.Id.Value, ride);
        }

        public async Task RemoveRide(RideModel ride)
        {
            Rides.Remove(ride);

            await storage.RemoveObject<SegmentModel>(ride.Id.Value);
        }

        public async Task SaveSegments(SegmentModel segment)
        {
            if (segment.Id == null)
            {
                segment.Id = Guid.NewGuid();

                Segments.Add(segment);
            }

            await storage.SaveObject(segment.Id.Value, segment);
        }

        public async Task RemoveSegments(SegmentModel segment)
        {
            Segments.Remove(segment);

            await storage.RemoveObject<SegmentModel>(segment.Id.Value);
        }
    }
}
