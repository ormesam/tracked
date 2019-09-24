using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Achievements;
using MtbMate.Contexts;
using MtbMate.Utilities;

namespace MtbMate.Models {
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
            var achievementResults = storage.GetAchievementResults();
            Achievements = LoadAchievements(achievementResults).ToObservable();
        }

        private IList<IAchievement> LoadAchievements(IList<AchievementResult> achievementResults) {
            var achievements = new List<IAchievement>() {
                new SpeedAchievement(1, 15),
                new SpeedAchievement(2, 18),
                new SpeedAchievement(3, 20),
                new SpeedAchievement(4, 22.5),
                new SpeedAchievement(5, 25),
                new JumpAchievement(6, 0.6),
                new JumpAchievement(7, 0.8),
                new JumpAchievement(8, 1),
                new JumpAchievement(9, 1.2),
                new JumpAchievement(10, 1.4),
                new JumpAchievement(11, 1.5),
            };

            foreach (var achievementResult in achievementResults) {
                var achievement = achievements
                    .SingleOrDefault(i => i.Id == achievementResult.AcheivementId);

                achievement.IsAchieved = true;
                achievement.Time = achievementResult.Time;
                achievement.RideId = achievementResult.RideId;
            }

            return achievements;
        }

        public async Task SaveRide(Ride ride) {
            if (ride.Id == null) {
                ride.Id = Guid.NewGuid();

                Rides.Add(ride);
            }

            await storage.SaveObject(ride.Id.Value, ride);
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

                await AnalyseExistingRides(segment);
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

        public async Task SaveSegmentAttempts(IList<SegmentAttempt> segmentAttempts) {
            foreach (var attempt in segmentAttempts) {
                await SaveSegmentAttempt(attempt);
            }
        }

        public async Task SaveSegmentAttempt(SegmentAttempt segmentAttempt) {
            if (segmentAttempt.Id == null) {
                segmentAttempt.Id = Guid.NewGuid();

                SegmentAttempts.Add(segmentAttempt);
            }

            await storage.SaveObject(segmentAttempt.Id.Value, segmentAttempt);
        }

        public async Task SaveAchievementResult(AchievementResult achievementResult) {
            if (achievementResult.Id == null) {
                achievementResult.Id = Guid.NewGuid();
            }

            await storage.SaveObject(achievementResult.Id.Value, achievementResult);
        }

        public void RemoveAchievementResults() {
            var results = storage.GetAchievementResults();

            foreach (var result in results) {
                storage.Storage.Invalidate(result.Id.Value.ToString());
            }

            foreach (var achievement in Achievements) {
                achievement.IsAchieved = false;
                achievement.RideId = null;
                achievement.Time = null;
            }
        }

#if DEBUG
        public async Task RunUtilityAsync() {
            // Perform single use operations here such as fixing data.

            await Task.CompletedTask;
        }
#endif
    }
}
