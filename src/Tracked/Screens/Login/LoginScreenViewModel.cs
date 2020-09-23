using System.Threading.Tasks;
using Tracked.Contexts;

namespace Tracked.Screens.Login {
    public class LoginScreenViewModel : ViewModelBase {
        public LoginScreenViewModel(MainContext context) : base(context) {
        }

        public async Task LoginWithGoogle() {
            await Context.Security.ConnectToGoogle();
        }
    }
}