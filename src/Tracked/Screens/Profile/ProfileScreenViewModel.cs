using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;

namespace Tracked.Screens.Profile {
    public class ProfileScreenViewModel : ViewModelBase {
        public ProfileScreenViewModel(MainContext context) : base(context) {
        }

        public ProfileDto User { get; private set; }

        public override string Title => "Title";

        public async Task Load() {
            User = await Context.Services.GetProfile();
        }
    }
}
