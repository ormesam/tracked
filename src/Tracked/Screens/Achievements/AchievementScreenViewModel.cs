using System.Threading.Tasks;
using Tracked.Contexts;
using Tracked.Models;

namespace Tracked.Screens.Achievements {
    public class AchievementScreenViewModel : ViewModelBase {
        //private readonly IAchievement achievement;

        public AchievementScreenViewModel(MainContext context) : base(context) {
            //this.achievement = achievement;
        }

        //public override string Title => achievement.Name;

        //public IEnumerable<Ride> Rides => achievement.GetRides();

        public async Task GoToRide(Ride ride) {
            await Context.UI.GoToRideReviewScreenAsync(ride);
        }
    }
}
