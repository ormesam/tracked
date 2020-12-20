using System.Threading.Tasks;
using Tracked.Contexts;

namespace Tracked.Screens.Login {
    public class LoginScreenViewModel : ViewModelBase {
        private bool isLoggingIn;

        public LoginScreenViewModel(MainContext context) : base(context) {
            isLoggingIn = false;
        }

        public async Task TryLogin() {
            IsLoggingIn = true;

            await Context.Security.Login();

            IsLoggingIn = false;
        }

        public bool IsLoggingIn {
            get { return isLoggingIn; }
            set {
                if (isLoggingIn != value) {
                    isLoggingIn = value;
                    OnPropertyChanged(nameof(IsLoggingIn));
                }
            }
        }
    }
}