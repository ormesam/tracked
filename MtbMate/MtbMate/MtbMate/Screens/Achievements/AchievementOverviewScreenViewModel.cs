using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MtbMate.Achievements;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;

namespace MtbMate.Screens.Review {
    public class AchievementOverviewScreenViewModel : ViewModelBase {
        public AchievementOverviewScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Achievements";

        public ObservableCollection<IAchievement> Achievements => Model.Instance.Achievements;

        public async Task GoToAchievement(INavigation nav, IAchievement achievement) {
            if (!achievement.HasBeenAchieved) {
                return;
            }

            await Context.UI.GoToAchievementScreenAsync(nav, achievement);
        }
    }
}
