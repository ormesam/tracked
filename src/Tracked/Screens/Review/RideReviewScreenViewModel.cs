using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Utilities;

namespace Tracked.Screens.Review {
    public class RideReviewScreenViewModel : ViewModelBase {
        private RideDto ride;

        public RideReviewScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Ride";

        public RideDto Ride {
            get { return ride; }
            set {
                if (ride != value) {
                    ride = value;
                    OnPropertyChanged();
                }
            }
        }

        public MapControlViewModel MapViewModel { get; set; }

        public int JumpCount => Ride.Jumps.Count;
        public string MaxAirtime => Ride.Jumps.Count == 0 ? "-" : $"{Ride.Jumps.Max(i => i.Airtime)}s";
        public bool HasJumps => JumpCount > 0;
        public bool HasSegments => Attempts.Count > 0;
        public IList<SegmentAttemptOverviewDto> Attempts => Ride.SegmentAttempts;
        public IList<JumpDto> Jumps => Ride.Jumps;

        public async Task Load(int id) {
            Ride = await Context.Services.GetRide(id);

            MapViewModel = new MapControlViewModel(
                Context,
                "Ride",
                PolyUtils.GetMapLocations(Ride.Locations, Ride.Jumps));

            OnPropertyChanged(nameof(MapViewModel));
        }

        public async Task GoToAttempt(SegmentAttemptOverviewDto attempt) {
            await Context.UI.GoToSegmentAttemptScreenAsync(attempt.SegmentAttemptId);
        }

        public async Task GoToSpeedAnalysis() {
            await Context.UI.GoToSpeedAnalysisScreenAsync(Ride.Locations);
        }

        public async Task CreateSegment() {
            await Context.UI.GoToCreateSegmentScreenAsync(Ride);
        }

        public async Task Delete() {
            await Context.Services.DeleteRide(Ride.RideId.Value);
        }
    }
}
