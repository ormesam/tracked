using System;
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
        public bool HasTrails => Attempts.Count > 0;
        public IList<TrailAttemptDto> Attempts => Ride.TrailAttempts;
        public IList<JumpDto> Jumps => Ride.Jumps;
        public bool CanCreateTrail => Context.Security.IsAdmin;

        public async Task Load(int id) {
            Ride = await Context.Services.GetRide(id);

            MapViewModel = new MapControlViewModel(
                Context,
                "Ride",
                PolyUtils.GetMapLocations(Ride.Locations, Ride.Jumps));

            MapViewModel.MapControlTapped += MapViewModel_MapControlTapped;

            OnPropertyChanged(nameof(MapViewModel));
        }

        private async void MapViewModel_MapControlTapped(object sender, EventArgs e) {
            await Context.UI.GoToMapScreenAsync(Ride);
        }

        public async Task GoToSpeedAnalysis() {
            await Context.UI.GoToSpeedAnalysisScreenAsync(Ride.Locations);
        }

        public async Task GoToTrail(TrailAttemptDto trailAttempt) {
            await Context.UI.GoToTrailScreenAsync(trailAttempt.TrailId);
        }

        public async Task CreateTrail() {
            await Context.UI.GoToCreateTrailScreenAsync(Ride);
        }

        public async Task Delete() {
            await Context.Services.DeleteRide(Ride.RideId.Value);
        }
    }
}
