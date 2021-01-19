using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;

namespace Tracked.Screens.Profile {
    public class ProfileScreenViewModel : TabbedViewModelBase {
        private ProfileDto user;

        public ProfileScreenViewModel(MainContext context) : base(context) {
        }

        protected override TabItemType SelectedTab => TabItemType.Profile;

        public ProfileDto User {
            get { return user; }
            set {
                if (user != value) {
                    user = value;
                    OnPropertyChanged(nameof(User));
                    OnPropertyChanged(nameof(Bio));
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

        public bool IsCurrentUser => User.UserId == Context.Security.UserId;

        public override string Title => IsCurrentUser ? "Profile" : User.Name;

        public async Task<bool> Load() {
            try {
                User = await Context.Services.GetProfile();

                return true;
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);

                return false;
            }
        }

        public async Task EditBio() {
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
    }
}
