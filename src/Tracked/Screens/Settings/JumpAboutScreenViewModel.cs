using Tracked.Contexts;

namespace Tracked.Screens.Settings {
    public class JumpAboutScreenViewModel : ViewModelBase {
        public JumpAboutScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Jump Detection";
    }
}
