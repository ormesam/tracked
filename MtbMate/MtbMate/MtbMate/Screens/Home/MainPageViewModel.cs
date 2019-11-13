using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Home {
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

                    await Sync();

                    Refresh();

                    IsRefreshing = false;
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

        private void Security_UserChanged(object sender, EventArgs e) {
            OnPropertyChanged(nameof(IsLoggedIn));
        }
    }
}