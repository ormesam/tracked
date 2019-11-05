using System;
using System.Diagnostics;
using MtbMate.Auth;
using Xamarin.Auth;

namespace MtbMate.Contexts {
    public class SecurityContext {
        public void ConnectToGoogle() {
            var authenticator = new OAuth2Authenticator(
                ApiKeysLocal.GoogleOAuthApiKey,
                null,
                "https://www.googleapis.com/auth/userinfo.email",
                new Uri("https://accounts.google.com/o/oauth2/auth"),
                new Uri(ApiKeysLocal.GoogleAuthRedirectUrl + ":/oauth2redirect"),
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
                string accessToken = e.Account.Properties["access_token"];

                Debug.WriteLine(accessToken);

                ////var request = new OAuth2Request("GET", new Uri("https://www.googleapis.com/oauth2/v2/userinfo"), null, e.Account);
                ////var response = await request.GetResponseAsync();
                ////if (response != null) {
                ////    string userJson = await response.GetResponseTextAsync();
                ////    var user = JsonConvert.DeserializeObject<GoogleUser>(userJson);

                ////    Debug.WriteLine(user.Email);
                ////}
            }
        }
    }
}
