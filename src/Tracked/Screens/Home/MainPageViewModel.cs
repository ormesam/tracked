using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Screens;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Home {
    public class MainPageViewModel : ViewModelBase {
        private bool isRefreshing;

        public MainPageViewModel(MainContext context) : base(context) {
            Context.Security.LoggedInStatusChanged += Security_UserChanged;
        }

        public bool IsRefreshing {
            get { return isRefreshing; }
            set {
                if (value != isRefreshing) {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        public ICommand RefreshCommand {
            get {
                return new Command(async () => {
                    IsRefreshing = true;

                    try {
                        await Sync();
                    } catch (ServiceException ex) {
                        Toast.LongAlert(ex.Message);
                    } finally {
                        Refresh();
                        IsRefreshing = false;
                    }
                });
            }
        }

        public ObservableCollection<Ride> Rides => Model.Instance.Rides
            .OrderByDescending(i => i.Start)
            .ToObservable();

        public bool HasRides => Rides.Any();

        public async Task GoToCreateRide(INavigation nav) {
            await Context.UI.GoToRecordScreenAsync(nav);
        }

        public async Task GoToReview(INavigation nav, Ride ride) {
            await Context.UI.GoToRideReviewScreenAsync(nav, ride);
        }

        public async Task Sync() {
            await SyncRides();
            await SyncSegments();
            await SyncSegmentAttempts();
        }

        private async Task SyncRides() {
            var ridesToSync = Model.Instance.Rides
                .Where(i => i.RideId == null);

            foreach (var ride in ridesToSync) {
                ride.RideId = await Context.Services.Sync(ride);

                await Model.Instance.SaveRide(ride);
            }

            var existingRideIds = Model.Instance.Rides
                .Where(row => row.RideId != null)
                .Select(row => row.RideId.Value)
                .ToList();

            var ridesToDownload = await Context.Services.GetRides(existingRideIds);

            foreach (var ride in ridesToDownload) {
                await Model.Instance.SaveRide(ride);
            }
        }

        private async Task SyncSegments() {
            var segmentsToSync = Model.Instance.Segments
                .Where(i => i.SegmentId == null);

            foreach (var segment in segmentsToSync) {
                segment.SegmentId = await Context.Services.Sync(segment);

                await Model.Instance.SaveSegment(segment);
            }

            var existingSegmentIds = Model.Instance.Segments
                .Where(row => row.SegmentId != null)
                .Select(row => row.SegmentId.Value)
                .ToList();

            var segmentsToDownload = await Context.Services.GetSegments(existingSegmentIds);

            foreach (var segment in segmentsToDownload) {
                await Model.Instance.SaveSegment(segment);
            }
        }

        private async Task SyncSegmentAttempts() {
            var attemptsToSync = Model.Instance.SegmentAttempts
                .Where(i => i.SegmentAttemptId == null);

            foreach (var attempt in attemptsToSync) {
                attempt.SegmentAttemptId = await Context.Services.Sync(attempt);

                await Model.Instance.SaveSegmentAttempt(attempt);
            }

            var existingAttemptIds = Model.Instance.SegmentAttempts
                .Where(row => row.SegmentAttemptId != null)
                .Select(row => row.SegmentAttemptId.Value)
                .ToList();

            var attemptsToDownload = await Context.Services.GetSegmentAttempts(existingAttemptIds);

            foreach (var attempt in attemptsToDownload) {
                await Model.Instance.SaveSegmentAttempt(attempt);
            }
        }

        private void Security_UserChanged(object sender, EventArgs e) {
            OnPropertyChanged(nameof(IsLoggedIn));
        }
    }
}