using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MtbMate.Auth;
using MtbMate.Dependancies;
using Shared;
using Xamarin.Auth;

namespace MtbMate.Contexts {
    public class SecurityContext {
        private MainContext mainContext;
        public string AccessToken { get; private set; }
        public string Name { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrEmpty(AccessToken);

        public event EventHandler LoggedInStatusChanged;

        public SecurityContext(MainContext mainContext) {
            this.mainContext = mainContext;

            AccessToken = this.mainContext.Storage.GetAccessToken();
            Name = this.mainContext.Storage.GetName();
        }

        public async Task SetAccessToken(string token, string name) {
            AccessToken = token;
            Name = name;
            await mainContext.Storage.SetAccessToken(token);
            await mainContext.Storage.SetName(name);

            LoggedInStatusChanged?.Invoke(null, null);
        }

        public async Task ClearAccessToken() {
            AccessToken = null;
            Name = null;
            await mainContext.Storage.SetAccessToken(null);
            await mainContext.Storage.SetName(null);

            LoggedInStatusChanged?.Invoke(null, null);
        }

        public void ConnectToGoogle() {
            var authenticator = new OAuth2Authenticator(
                Constants.GoogleOAuthApiKey,
                null,
                "https://www.googleapis.com/auth/userinfo.email",
                new Uri("https://accounts.google.com/o/oauth2/auth"),
                new Uri(Constants.GoogleAuthRedirectUrl + ":/oauth2redirect"),
                new Uri("https://www.googleapis.com/oauth2/v4/token"),
                null,
                true);

            authenticator.Completed += Authenticator_Completed;
            authenticator.Error += Authenticator_Error;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        private void Authenticator_Error(object sender, AuthenticatorErrorEventArgs e) {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null) {
                authenticator.Completed -= Authenticator_Completed;
                authenticator.Error -= Authenticator_Error;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
        }

        private async void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e) {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null) {
                authenticator.Completed -= Authenticator_Completed;
                authenticator.Error -= Authenticator_Error;
            }

            if (e.IsAuthenticated) {
                string accessToken = e.Account.Properties["id_token"];

                var loginResponse = await mainContext.Services.Login(accessToken);

                await SetAccessToken(loginResponse.AccessToken, loginResponse.Name);

                Toast.LongAlert("Connected to Google");
            }
        }
    }
}
