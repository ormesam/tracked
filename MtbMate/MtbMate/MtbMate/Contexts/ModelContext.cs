using MtbMate.Models;
using MtbMate.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MtbMate.Contexts
{
    public class ModelContext
    {
        private StorageContext storage;

        public ObservableCollection<RideModel> Rides { get; set; }
        public ObservableCollection<SegmentModel> Segments { get; set; }
        public ObservableCollection<SegmentAttemptModel> SegmentAttempts { get; set; }

        public ModelContext(MainContext mainContext) {
            storage = mainContext.Storage;

            Rides = storage.GetRides().ToObservable();
            Segments = storage.GetSegments().ToObservable();
            SegmentAttempts = storage.GetSegmentAttempts().ToObservable();
        }

        public async Task SaveRide(RideModel ride) {
            if (ride.Id == null) {
                ride.Id = Guid.NewGuid();

                Rides.Add(ride);
            }

            await storage.SaveObject(ride.Id.Value, ride);
        }

        public async Task RemoveRide(RideModel ride) {
            var attempts = SegmentAttempts
                .Where(i => i.RideId == ride.Id)
                .ToList(); ;

            await RemoveSegmentAttempts(attempts);

            Rides.Remove(ride);

            await storage.RemoveObject<SegmentModel>(ride.Id.Value);
        }

        public async Task SaveSegment(SegmentModel segment) {
            if (segment.Id == null) {
                segment.Id = Guid.NewGuid();

                Segments.Add(segment);

                await AnalyseExistingRides(segment);
            }

            await storage.SaveObject(segment.Id.Value, segment);
        }

        private async Task AnalyseExistingRides(SegmentModel segment) {
            foreach (var ride in Rides) {
                SegmentAttemptModel result = ride.MatchesSegment(segment);

                if (result != null) {
                    await SaveSegmentAttempt(result);
                }
            }
        }

        public async Task RemoveSegment(SegmentModel segment) {
            var attempts = SegmentAttempts
                .Where(i => i.SegmentId == segment.Id)
                .ToList();

            await RemoveSegmentAttempts(attempts);

            Segments.Remove(segment);

            await storage.RemoveObject<SegmentModel>(segment.Id.Value);
        }

        private async Task RemoveSegmentAttempts(IEnumerable<SegmentAttemptModel> attempts) {
            foreach (var attempt in attempts) {
                SegmentAttempts.Remove(attempt);
                await storage.RemoveObject<SegmentAttemptModel>(attempt.Id.Value);
            }
        }

        public async Task SaveSegmentAttempt(SegmentAttemptModel segmentAttempt) {
            if (segmentAttempt.Id == null) {
                segmentAttempt.Id = Guid.NewGuid();

                SegmentAttempts.Add(segmentAttempt);
            }

            await storage.SaveObject(segmentAttempt.Id.Value, segmentAttempt);
        }
    }
}
