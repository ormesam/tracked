using System.Threading.Tasks;
using Tracked.Auth;
using Tracked.Screens.Master;
using Tracked.Utilities;

namespace Tracked.Contexts {
    public class SecurityContext {
        private readonly MainContext mainContext;
        public string AccessToken { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrEmpty(AccessToken) && CrossGoogleClient.Current.IsLoggedIn;

        public SecurityContext(MainContext mainContext) {
            this.mainContext = mainContext;

            AccessToken = this.mainContext.Storage.GetAccessToken();
        }

        public async Task SetAccessToken(string token) {
            AccessToken = token;
            await mainContext.Storage.SetAccessToken(token);
        }

        public async Task Logout() {
            AccessToken = null;
            await mainContext.Storage.SetAccessToken(null);
            CrossGoogleClient.Current.Logout();
        }

        public async Task ConnectToGoogle() {
            if (CrossGoogleClient.Current.IsLoggedIn) {
                return;
            }

            var result = await CrossGoogleClient.Current.LoginAsync();

            if (result.Status == GoogleActionStatus.Completed) {
                var loginResponse = await mainContext.Services.Login(CrossGoogleClient.Current.AccessToken, result.User);

                await SetAccessToken(loginResponse.AccessToken);

                Toast.LongAlert("Connected to Google");

                App.Current.MainPage = new MasterScreen(mainContext);

                return;
            }

            Toast.LongAlert("Unable to connect to Google\nTry again later");
        }
    }
}
