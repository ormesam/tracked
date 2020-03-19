using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Screens.Review {
    public class AchievementOverviewScreenViewModel : ViewModelBase {
        private bool isRefreshing;

        public AchievementOverviewScreenViewModel(MainContext context) : base(context) {
            Achievements = new List<AchievementDto>();
        }

        public override string Title => "Achievements";

        public IList<AchievementDto> Achievements { get; set; }

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

        public async Task Load() {
            IsRefreshing = true;

            try {
                Achievements = await Context.Services.GetAchievements();
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            } finally {
                IsRefreshing = false;
                Refresh();
            }
        }

        public async Task GoToAchievement(AchievementDto achievement) {
            if (!achievement.HasBeenAchieved) {
                return;
            }

            await Context.UI.GoToAchievementScreenAsync(achievement);
        }
    }
}
