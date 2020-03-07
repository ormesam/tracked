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
                        // await Sync();
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

        public async Task GoToCreateRide() {
            await Context.UI.GoToRecordScreenAsync();
        }

        public async Task GoToReview(Ride ride) {
            await Context.UI.GoToRideReviewScreenAsync(ride);
        }

        private void Security_UserChanged(object sender, EventArgs e) {
            OnPropertyChanged(nameof(IsLoggedIn));
        }
    }
}