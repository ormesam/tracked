using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Models;
using Tracked.Utilities;

namespace Tracked.Screens.Review {
    public class RideReviewScreenViewModel : ViewModelBase {
        private RideDto ride;

        public RideReviewScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => Ride.DisplayName;

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

        public string Time => (Ride.EndUtc - Ride.StartUtc).ToString(@"mm\:ss");
        public int JumpCount => Ride.Jumps.Count;
        public string MaxAirtime => Ride.Jumps.Count == 0 ? "-" : $"{Ride.Jumps.Max(i => i.Airtime)}s";
        public IList<SegmentAttemptOverviewDto> Attempts => Ride.SegmentAttempts;
        public IList<RideJumpDto> Jumps => Ride.Jumps;

        public async Task Load(int id) {
            Ride = await Context.Services.GetRide(id);

            MapViewModel = new MapControlViewModel(
                Context,
                Ride.DisplayName,
                PolyUtils.GetMapLocations(Ride));

            OnPropertyChanged(nameof(MapViewModel));
        }

        public async Task GoToAttempt(SegmentAttempt attempt) {
            // await Context.UI.GoToSegmentAttemptScreenAsync(attempt);
        }

        public async Task GoToSpeedAnalysis() {
            //  await Context.UI.GoToSpeedAnalysisScreenAsync(Ride);
        }

        public async Task ViewJumpBreakdown() {
            // await Context.UI.GoToAccelerometerReadingsScreenAsync(Ride);
        }
    }
}
