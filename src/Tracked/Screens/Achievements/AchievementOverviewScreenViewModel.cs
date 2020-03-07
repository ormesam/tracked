using Tracked.Contexts;

namespace Tracked.Screens.Review {
    public class AchievementOverviewScreenViewModel : ViewModelBase {
        public AchievementOverviewScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Achievements";

        ////public ObservableCollection<IAchievement> Achievements => Model.Instance.Achievements;

        ////public async Task GoToAchievement(IAchievement achievement) {
        ////    if (!achievement.HasBeenAchieved) {
        ////        return;
        ////    }

        ////    await Context.UI.GoToAchievementScreenAsync(achievement);
        ////}
    }
}
