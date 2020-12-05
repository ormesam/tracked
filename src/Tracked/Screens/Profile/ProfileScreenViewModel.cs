using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;

namespace Tracked.Screens.Profile {
    public class ProfileScreenViewModel : ViewModelBase {
        private ProfileDto user;

        public ProfileScreenViewModel(MainContext context) : base(context) {
        }

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

        public string Bio {
            get {
                if (string.IsNullOrWhiteSpace(User.Bio)) {
                    return "Apparently, this user prefers to keep an air of mystery about them.";
                }

                return User.Bio;
            }
        }

        public bool IsCurrentUser => User.UserId == Context.Security.UserId;

        public override string Title => IsCurrentUser ? "Profile" : User.Name;

        public async Task Load() {
            User = await Context.Services.GetProfile();
        }
    }
}
