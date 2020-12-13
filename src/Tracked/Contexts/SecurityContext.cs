using System.Threading.Tasks;
using Shared.Dto;
using Shared.Dtos;
using Shared.Exceptions;
using Tracked.Auth;
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
            GoogleResponse result;

            try {
                result = await CrossGoogleClient.Current.LoginAsync();
            } catch {
                Toast.LongAlert("Unable to connect to Google\nTry again later");

                return;
            }

            if (result.Status == GoogleActionStatus.Completed) {
                await Login(result.User);
            }
        }

        private async Task Login(GoogleUserDto user) {
            var loginResponse = await mainContext.Services.Login(CrossGoogleClient.Current.IdToken, user);

            Login(loginResponse.AccessToken, loginResponse.User);

            await mainContext.UI.GoToRideOverviewScreenAsync();
        }

        private void Login(string token, UserDto user) {
            AccessToken = token;
            this.user = user;
        }
    }
}
