using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Screens;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Home {
    public class MainPageViewModel : ViewModelBase {
        private bool isRefreshing;

        public MainPageViewModel(MainContext context) : base(context) {
            Rides = new List<RideOverviewDto>();
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
            get { return new Command(async () => await Load()); }
        }

        public IList<RideOverviewDto> Rides { get; set; }

        public bool HasRides => Rides.Any();

        public async Task Load() {
            IsRefreshing = true;

            try {
                Rides = await Context.Services.GetRideOverviews();
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            } finally {
                IsRefreshing = false;
                Refresh();
            }
        }

        public async Task GoToCreateRide() {
            await Context.UI.GoToRecordScreenAsync();
        }

        public async Task GoToReview(RideOverviewDto ride) {
            await Context.UI.GoToRideReviewScreenAsync(ride.RideId);
        }
    }
}