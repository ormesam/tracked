using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Utilities;

namespace Tracked.Screens.Rides {
    public class RideReviewScreenViewModel : RideMapViewModelBase {
        private RideDto ride;
        private int? latestAnalyserVersion;

        public RideReviewScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Ride";

        protected override bool IsReadOnly => true;

        public override RideDto Ride => ride;

        public int? LatestAnalyserVersion => latestAnalyserVersion;
        public int JumpCount => Ride.Jumps.Count;
        public string MaxAirtime => Ride.Jumps.Count == 0 ? "-" : $"{Ride.Jumps.Max(i => i.Airtime)}s";
        public bool HasJumps => Ride.Jumps.Any();
        public bool HasTrails => Attempts.Any();
        public bool HasAchievements => Achievements.Any();
        public IList<TrailAttemptDto> Attempts => Ride.TrailAttempts;
        public IList<JumpDto> Jumps => Ride.Jumps;
        public IList<AchievementDto> Achievements => Ride.Achievements;
        public bool CanCreateTrail => Context.Security.IsAdmin;

        public async Task<bool> Load(int id) {
            try {
                ride = await Context.Services.GetRide(id);
                latestAnalyserVersion = await Context.Services.GetLatestAnalyserVersion();

                OnPropertyChanged();

                return true;
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);

                return false;
            }
        }

        private async void MapViewModel_MapControlTapped(object sender, EventArgs e) {
            await Context.UI.GoToMapScreenAsync(Ride);
        }

        public async Task GoToSpeedAnalysis() {
            await Context.UI.GoToSpeedAnalysisScreenAsync(Ride.Locations);
        }

        public async Task GoToTrailScreen(TrailAttemptDto trailAttempt) {
            await Context.UI.GoToTrailScreenAsync(trailAttempt.TrailId);
        }

        public async Task CreateTrail() {
            await Context.UI.GoToCreateTrailScreenAsync(Ride);
        }

        public async Task Delete() {
            try {
                await Context.Services.DeleteRide(Ride.RideId.Value);
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            }
        }

        public async Task GoToMapScreen() {
            await Context.UI.GoToMapScreenAsync(Ride);
        }

        public async Task Reanalyse() {
            await Context.Services.ReanalyseRide(Ride.RideId.Value);
        }
    }
}
