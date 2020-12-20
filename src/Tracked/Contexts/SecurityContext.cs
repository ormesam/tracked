using System.Threading.Tasks;
using Shared.Dtos;
using Shared.Exceptions;
using Tracked.Auth;
using Tracked.Utilities;
using Xamarin.Essentials;

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

        public async Task Login() {
            string refreshToken = await GetRefreshToken();

            if (string.IsNullOrWhiteSpace(refreshToken)) {
                await LoginWithGoogle();
            } else {
                await LoginWithToken(refreshToken);
            }
        }

        private async Task LoginWithGoogle() {
            GoogleResponse googleAuthResponse;

            try {
                // Logout to force the google account selector to open
                CrossGoogleClient.Current.Logout();
                googleAuthResponse = await CrossGoogleClient.Current.LoginAsync();
            } catch {
                Toast.LongAlert("Unable to connect to Google\nTry again later");

                return;
            }

            if (googleAuthResponse.Status == GoogleActionStatus.Completed) {
                var loginResponse = await mainContext.Services.Authenticate(CrossGoogleClient.Current.IdToken, googleAuthResponse.User);

                await ReceiveLoginResponse(loginResponse);
            }
        }

        private async Task LoginWithToken(string refreshToken) {
            var loginResponse = await mainContext.Services.Login(refreshToken);

            await ReceiveLoginResponse(loginResponse);
        }

        private async Task ReceiveLoginResponse(LoginResponseDto loginResponse) {
            await SetRefreshToken(loginResponse.RefreshToken);
            AccessToken = loginResponse.AccessToken;
            this.user = loginResponse.User;

            await mainContext.UI.GoToRideOverviewScreenAsync();
        }

        internal Task<string> GetRefreshToken() {
            return SecureStorage.GetAsync("RefreshToken");
        }

        internal Task SetRefreshToken(string token) {
            return SecureStorage.SetAsync("RefreshToken", token);
        }
    }
}
