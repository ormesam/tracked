using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Achievements;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Screens.Review {
    public class AchievementScreenViewModel : ViewModelBase {
        public AchievementScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Achievements";

        public ObservableCollection<IAchievement> Achievements => Model.Instance.Achievements;

        public async Task GoToRide(INavigation nav, IAchievement achievement) {
            if (achievement.RideId == null) {
                return;
            }

            var ride = Model.Instance.Rides.SingleOrDefault(i => i.Id == achievement.RideId);

            await Context.UI.GoToReviewScreenAsync(nav, ride);
        }

        public async Task ReCompareRides() {
            await AchievementUtility.ReanalyseAchievementResults();

            OnPropertyChanged();
        }
    }
}
