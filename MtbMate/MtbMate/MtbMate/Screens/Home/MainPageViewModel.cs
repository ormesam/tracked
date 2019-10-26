using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Home {
    public class MainPageViewModel : ViewModelBase {
        public MainPageViewModel(MainContext context) : base(context) {
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
    }
}