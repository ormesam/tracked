using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;

namespace Tracked.Screens.Profile {
    public class ProfileScreenViewModel : ViewModelBase {
        public ProfileScreenViewModel(MainContext context) : base(context) {
        }

        public ProfileDto User { get; private set; }

        public string Bio {
            get {
                if (string.IsNullOrWhiteSpace(User.Bio)) {
                    return "Apparently, this user prefers to keep an air of mystery about them.";
                }

                return User.Bio;
            }
        }

        public override string Title => "Profile";

        public async Task Load() {
            User = await Context.Services.GetProfile();
        }
    }
}
