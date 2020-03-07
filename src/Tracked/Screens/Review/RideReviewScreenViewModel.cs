using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Models;
using Tracked.Utilities;

namespace Tracked.Screens.Review {
    public class RideReviewScreenViewModel : ViewModelBase {
        public readonly IRide Ride;

        public RideReviewScreenViewModel(MainContext context, IRide ride) : base(context) {
            Ride = ride;

            MapViewModel = new MapControlViewModel(
                context,
                Ride.DisplayName,
                PolyUtils.GetMapLocations(Ride));
        }

        public override string Title => DisplayName;

        public string DisplayName => Ride.DisplayName;

        public MapControlViewModel MapViewModel { get; }

        public double AverageSpeed {
            get {
                if (!Ride.Locations.Any()) {
                    return 0;
                }

                return Ride.Locations.Average(i => i.Mph);
            }
        }

        public double MaxSpeed {
            get {
                if (!Ride.Locations.Any()) {
                    return 0;
                }

                return Ride.Locations.Max(i => i.Mph);
            }
        }

        public double Distance => 0;
        public string Time => (Ride.End - Ride.Start).ToString(@"mm\:ss");
        public int JumpCount => Ride.Jumps.Count;
        public string MaxAirtime => Ride.Jumps.Count == 0 ? "-" : $"{Ride.Jumps.Max(i => i.Airtime)}s";
        public bool ShowAttempts => Ride.ShowAttempts;

        public IList<SegmentAttempt> Attempts => !ShowAttempts ? new List<SegmentAttempt>() :
            Model.Instance.SegmentAttempts
                .Where(i => i.RideId == Ride.Id)
                .OrderByDescending(i => i.Start)
                .ToList();

        public IList<Jump> Jumps => Ride.Jumps
            .OrderBy(i => i.Timestamp)
            .ToList();

        public async Task Delete() {
            await Model.Instance.RemoveRide(Ride as Ride);
        }

        public async Task GoToAttempt(SegmentAttempt attempt) {
            await Context.UI.GoToSegmentAttemptScreenAsync(attempt);
        }

        public async Task GoToSpeedAnalysis() {
            await Context.UI.GoToSpeedAnalysisScreenAsync(Ride);
        }

        public async Task ViewJumpBreakdown() {
            await Context.UI.GoToAccelerometerReadingsScreenAsync(Ride);
        }
    }
}
