using System.Collections.Generic;
using System.Threading.Tasks;
using MtbMate.Achievements;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;

namespace MtbMate.Screens.Achievements {
    public class AchievementScreenViewModel : ViewModelBase {
        private readonly IAchievement achievement;

        public AchievementScreenViewModel(MainContext context, IAchievement achievement) : base(context) {
            this.achievement = achievement;
        }

        public override string Title => achievement.Name;

        public IEnumerable<Ride> Rides => achievement.GetRides();

        public async Task GoToRide(INavigation nav, Ride ride) {
            await Context.UI.GoToRideReviewScreenAsync(nav, ride);
        }
    }
}
