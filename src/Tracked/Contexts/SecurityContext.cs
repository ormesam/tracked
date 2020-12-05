using System.Threading.Tasks;
using Shared.Dto;
using Shared.Dtos;
using Shared.Exceptions;
using Tracked.Auth;
using Tracked.Screens.Master;
using Tracked.Utilities;

namespace Tracked.Contexts {
    public class SecurityContext {
        private readonly MainContext mainContext;
        private UserDto user;

        internal string AccessToken { get; private set; }

        internal int UserId {
            get {
                if (user == null) {
                    throw new NotLoggedInException();
                }

                return user.UserId.Value;
            }
        }

        internal bool IsAdmin => user?.IsAdmin ?? false;

        public SecurityContext(MainContext mainContext) {
            this.mainContext = mainContext;
        }

        public void Logout() {
            AccessToken = null;
            user = null;

            CrossGoogleClient.Current.Logout();
        }

        public async Task ConnectToGoogle() {
            if (!CrossGoogleClient.Current.IsLoggedIn || string.IsNullOrWhiteSpace(CrossGoogleClient.Current.IdToken)) {
                GoogleResponse result;

                try {
                    result = await CrossGoogleClient.Current.LoginAsync();
                } catch {
                    return;
                }

                if (result.Status == GoogleActionStatus.Completed) {
                    await Login(result.User);

                    return;
                }
            } else {
                await Login(CrossGoogleClient.Current.CurrentUser);

                return;
            }

            Toast.LongAlert("Unable to connect to Google\nTry again later");
        }

        private async Task Login(GoogleUserDto user) {
            var loginResponse = await mainContext.Services.Login(CrossGoogleClient.Current.IdToken, user);

            Login(loginResponse.AccessToken, loginResponse.User);

            Toast.LongAlert("Connected to Google");

            App.Current.MainPage = new MasterScreen(mainContext);
        }

        private void Login(string token, UserDto user) {
            AccessToken = token;
            this.user = user;
        }
    }
}
