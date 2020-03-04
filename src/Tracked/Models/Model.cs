using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Tracked.Achievements;
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

        public ObservableCollection<Ride> Rides { get; set; }
        public ObservableCollection<Segment> Segments { get; set; }
        public ObservableCollection<SegmentAttempt> SegmentAttempts { get; set; }
        public ObservableCollection<IAchievement> Achievements { get; set; }

        private Model() {
        }

        public void Init(MainContext mainContext) {
            storage = mainContext.Storage;

            Rides = storage.GetRides().ToObservable();
            Segments = storage.GetSegments().ToObservable();
            SegmentAttempts = storage.GetSegmentAttempts().ToObservable();
            Achievements = LoadAchievements().ToObservable();
        }

        private IList<IAchievement> LoadAchievements() {
            return new List<IAchievement>() {
                new SpeedAchievement(18),
                new SpeedAchievement(20),
                new SpeedAchievement(22),
                new SpeedAchievement(24),
                new SpeedAchievement(26),
                new SpeedAchievement(28),
                new SpeedAchievement(30),
                new JumpAchievement(0.6),
                new JumpAchievement(0.8),
                new JumpAchievement(1),
                new JumpAchievement(1.2),
                new JumpAchievement(1.4),
                new JumpAchievement(1.6),
                new JumpAchievement(1.8),
                new JumpAchievement(2),
            };
        }

        public async Task SaveRide(Ride ride) {
            if (ride.Id == null) {
                ride.Id = Guid.NewGuid();

                Rides.Add(ride);
            }

            await storage.SaveObject(ride.Id.Value, ride);
        }

        public async Task CompareSegments(Ride ride) {
            foreach (var segment in Segments) {
                SegmentAttempt result = ride.MatchesSegment(segment);

                if (result != null) {
                    await SaveSegmentAttempt(result);
                }
            }
        }

        public async Task RemoveRide(Ride ride) {
            var attempts = SegmentAttempts
                .Where(i => i.RideId == ride.Id)
                .ToList(); ;

            await RemoveSegmentAttempts(attempts);

            Rides.Remove(ride);

            await storage.RemoveObject<Segment>(ride.Id.Value);
        }

        public async Task SaveSegment(Segment segment) {
            if (segment.Id == null) {
                segment.Id = Guid.NewGuid();

                Segments.Add(segment);
            }

            await storage.SaveObject(segment.Id.Value, segment);
        }

        public async Task AnalyseExistingRides(Segment segment) {
            foreach (var ride in Rides.OrderBy(i => i.Start)) {
                SegmentAttempt result = ride.MatchesSegment(segment);

                if (result != null) {
                    await SaveSegmentAttempt(result);
                }
            }
        }

        public async Task RemoveSegment(Segment segment) {
            var attempts = SegmentAttempts
                .Where(i => i.SegmentId == segment.Id)
                .ToList();

            await RemoveSegmentAttempts(attempts);

            Segments.Remove(segment);

            await storage.RemoveObject<Segment>(segment.Id.Value);
        }

        public async Task RemoveSegmentAttempts(IEnumerable<SegmentAttempt> attempts) {
            foreach (var attempt in attempts) {
                SegmentAttempts.Remove(attempt);
                await storage.RemoveObject<SegmentAttempt>(attempt.Id.Value);
            }
        }

        public async Task SaveSegmentAttempt(SegmentAttempt segmentAttempt) {
            if (segmentAttempt.Id == null) {
                segmentAttempt.Id = Guid.NewGuid();

                SegmentAttempts.Add(segmentAttempt);
            }

            await storage.SaveObject(segmentAttempt.Id.Value, segmentAttempt);
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
