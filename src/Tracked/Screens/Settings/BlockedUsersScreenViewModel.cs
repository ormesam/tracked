using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;

namespace Tracked.Screens.Settings {
    public class BlockedUsersScreenViewModel : TabbedViewModelBase {
        private ObservableCollection<BlockedUserDto> blockedUsers;

        public BlockedUsersScreenViewModel(MainContext context) : base(context, TabItemType.Settings) {
            blockedUsers = new ObservableCollection<BlockedUserDto>();
        }

        public override string Title => "Blocked Users";

        public ObservableCollection<BlockedUserDto> BlockedUsers {
            get { return blockedUsers; }
            set {
                if (blockedUsers != value) {
                    blockedUsers = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task<bool> Load() {
            var blockedUsers = await Context.Services.GetBlockedUsersAsync();

            BlockedUsers = blockedUsers.ToObservable();

            return true;
        }

        public async Task Unblock(BlockedUserDto user) {
            await Context.Services.Unblock(user.UserId);

            BlockedUsers.Remove(user);
        }
    }
}
