using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;

namespace Tracked.Screens.Profile {
    public class ProfileScreenViewModel : TabbedViewModelBase {
        private ProfileDto user;

        public ProfileScreenViewModel(MainContext context, TabItemType selectedTab) : base(context, selectedTab) {
        }

        public ProfileDto User {
            get { return user; }
            set {
                if (user != value) {
                    user = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BioTitle {
            get {
                if (IsCurrentUser) {
                    return "Bio (Tap to edit)";
                }

                return "Bio";
            }
        }

        public string Bio {
            get {
                if (string.IsNullOrWhiteSpace(User.Bio)) {
                    if (IsCurrentUser) {
                        return "Why not tell us something about yourself?";
                    }

                    return "Apparently, this user prefers to keep an air of mystery about them.";
                }

                return User.Bio;
            }
        }

        public bool IsCurrentUser => User?.UserId == Context.Security.UserId;

        public bool IsFollowing => IsCurrentUser ? true : User.IsFollowing;

        public override string Title => IsCurrentUser ? "Profile" : User.Name;

        public async Task<bool> Load(int userId) {
            try {
                User = await Context.Services.GetProfile(userId);

                return true;
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);

                return false;
            }
        }

        public async Task EditBio() {
            if (!IsCurrentUser) {
                return;
            }

            string newBio = await Context.UI.ShowPromptAsync("Bio", "Tell us something about yourself", User.Bio);

            if (newBio == null) {
                return;
            }

            try {
                await Context.Services.UpdateBio(newBio);

                User.Bio = newBio;

                OnPropertyChanged(nameof(Bio));
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            }
        }

        public async Task Follow() {
            if (IsCurrentUser || User.IsFollowing) {
                return;
            }

            await Context.Services.Follow(User.UserId);

            User.IsFollowing = true;

            OnPropertyChanged();
        }

        public async Task Unfollow() {
            if (IsCurrentUser || !User.IsFollowing) {
                return;
            }

            await Context.Services.Unfollow(User.UserId);

            User.IsFollowing = false;

            OnPropertyChanged();
        }

        public async Task Block() {
            if (IsCurrentUser) {
                return;
            }

            await Context.Services.Block(User.UserId);

            await Context.UI.GoToFeedScreen();
        }
    }
}
