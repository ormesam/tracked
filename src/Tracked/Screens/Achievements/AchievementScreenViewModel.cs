using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;

namespace Tracked.Screens.Achievements {
    public class AchievementScreenViewModel : ViewModelBase {
        public AchievementScreenViewModel(MainContext context, AchievementDto achievement) : base(context) {
            Achievement = achievement;
        }

        public override string Title => Achievement.Name;
        public AchievementDto Achievement { get; set; }

        public async Task GoToRide(RideOverviewDto ride) {
            await Context.UI.GoToRideReviewScreenAsync(ride.RideId);
        }
    }
}
