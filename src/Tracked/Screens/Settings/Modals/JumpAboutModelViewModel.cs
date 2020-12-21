using System.Threading.Tasks;
using Tracked.Contexts;

namespace Tracked.Screens.Settings.Modals {
    public class JumpAboutModelViewModel : ViewModelBase {
        public JumpAboutModelViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Jump Detection";

        public async Task GoToAboutJump() {
            await Context.UI.GoToJumpAboutScreenAsync();
        }
    }
}
