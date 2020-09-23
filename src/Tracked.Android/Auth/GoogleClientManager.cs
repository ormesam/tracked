using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Tasks;
using Java.Interop;
using Shared.Dto;
using Tracked.Auth;

namespace Tracked.Droid.Auth {

    public class GoogleClientManager : Java.Lang.Object, IGoogleClientManager, IOnCompleteListener {
        private readonly GoogleSignInClient googleSignInClient;

        private static readonly int authActivityID = 9637;
        private static TaskCompletionSource<GoogleResponse> loginTcs;
        private static string idToken;
        private static string accessToken;
        private static Activity currentActivity;

        public string IdToken => idToken;
        public string AccessToken => accessToken;

        public GoogleUserDto CurrentUser {
            get {
                GoogleSignInAccount userAccount = GoogleSignIn.GetLastSignedInAccount(currentActivity);

                if (userAccount == null) {
                    return null;
                }

                return new GoogleUserDto {
                    Id = userAccount.Id,
                    Name = userAccount.DisplayName,
                    GivenName = userAccount.GivenName,
                    FamilyName = userAccount.FamilyName,
                    Email = userAccount.Email,
                    Picture = new Uri(userAccount.PhotoUrl != null ? $"{userAccount.PhotoUrl}" : string.Empty)
                };
            }
        }

        private GoogleClientManager() {
            if (currentActivity == null) {
                throw new GoogleClientNotInitializedErrorException(GoogleClientBaseException.ClientNotInitializedErrorMessage);
            }

            var googleSignInOptions = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail()
                .RequestScopes(new Scope(Scopes.Profile))
                .Build();

            googleSignInClient = GoogleSignIn.GetClient(currentActivity, googleSignInOptions);
        }

        public static void Init(Activity activity) {
            currentActivity = activity;
            CrossGoogleClient.SetInstance(new GoogleClientManager());
        }

        public async Task<GoogleResponse> LoginAsync() {
            if (currentActivity == null || googleSignInClient == null) {
                throw new GoogleClientNotInitializedErrorException(GoogleClientBaseException.ClientNotInitializedErrorMessage);
            }

            loginTcs = new TaskCompletionSource<GoogleResponse>();

            GoogleSignInAccount account = GoogleSignIn.GetLastSignedInAccount(currentActivity);

            if (account != null) {
                OnSignInSuccessful(account);
            } else {
                Intent intent = googleSignInClient.SignInIntent;
                currentActivity?.StartActivityForResult(intent, authActivityID);
            }

            return await loginTcs.Task;
        }

        public async Task<GoogleResponse> SilentLoginAsync() {
            if (currentActivity == null || googleSignInClient == null) {
                throw new GoogleClientNotInitializedErrorException(GoogleClientBaseException.ClientNotInitializedErrorMessage);
            }

            loginTcs = new TaskCompletionSource<GoogleResponse>();

            GoogleSignInAccount account = GoogleSignIn.GetLastSignedInAccount(currentActivity);

            if (account != null) {
                OnSignInSuccessful(account);
            } else {
                GoogleSignInAccount userAccount = await googleSignInClient.SilentSignInAsync();
                OnSignInSuccessful(userAccount);
            }

            return await loginTcs.Task;
        }

        public void Logout() {
            if (currentActivity == null || googleSignInClient == null) {
                throw new GoogleClientNotInitializedErrorException(GoogleClientBaseException.ClientNotInitializedErrorMessage);
            }

            if (GoogleSignIn.GetLastSignedInAccount(currentActivity) != null) {
                idToken = null;
                accessToken = null;
                googleSignInClient.SignOut();
            }
        }

        public bool IsLoggedIn {
            get {
                if (currentActivity == null) {
                    throw new GoogleClientNotInitializedErrorException(GoogleClientBaseException.ClientNotInitializedErrorMessage);
                }

                return GoogleSignIn.GetLastSignedInAccount(currentActivity) != null;
            }
        }

        public static void OnAuthCompleted(int requestCode, Intent data) {
            if (requestCode != authActivityID) {
                return;
            }

            GoogleSignIn.GetSignedInAccountFromIntent(data).AddOnCompleteListener(CrossGoogleClient.Current as IOnCompleteListener);
        }

        private async void OnSignInSuccessful(GoogleSignInAccount userAccount) {
            GoogleUserDto user = new GoogleUserDto {
                Id = userAccount.Id,
                Name = userAccount.DisplayName,
                GivenName = userAccount.GivenName,
                FamilyName = userAccount.FamilyName,
                Email = userAccount.Email,
                Picture = new Uri(userAccount.PhotoUrl != null ? $"{userAccount.PhotoUrl}" : string.Empty),
            };

            idToken = userAccount.IdToken;

            if (userAccount.GrantedScopes != null && userAccount.GrantedScopes.Count > 0) {
                var scopes = $"oauth2:{string.Join(' ', userAccount.GrantedScopes.Select(s => s.ScopeUri).ToArray())}";
                var tcs = new TaskCompletionSource<string>();

                await System.Threading.Tasks.Task.Run(() => {
                    try {
                        tcs.TrySetResult(GoogleAuthUtil.GetToken(Application.Context, userAccount.Account, scopes));
                    } catch (Exception ex) {
                        tcs.TrySetResult(string.Empty);
                        Console.WriteLine($"Ex: {ex}");
                    }
                });

                accessToken = await tcs.Task;
            }

            var googleArgs = new GoogleClientResultEventArgs(user, GoogleActionStatus.Completed);

            // Send the result to the receivers
            loginTcs.TrySetResult(new GoogleResponse(googleArgs));
        }

        private void OnSignInFailed(ApiException apiException) {
            GoogleClientErrorEventArgs errorEventArgs = new GoogleClientErrorEventArgs();
            Exception exception;

            switch (apiException.StatusCode) {
                case CommonStatusCodes.InternalError:
                    errorEventArgs.Error = GoogleClientErrorType.SignInInternalError;
                    errorEventArgs.Message = GoogleClientBaseException.SignInInternalErrorMessage;
                    exception = new GoogleClientSignInInternalErrorException();
                    break;
                case CommonStatusCodes.ApiNotConnected:
                    errorEventArgs.Error = GoogleClientErrorType.SignInApiNotConnectedError;
                    errorEventArgs.Message = GoogleClientBaseException.SignInApiNotConnectedErrorMessage;
                    exception = new GoogleClientSignInApiNotConnectedErrorException();
                    break;
                case CommonStatusCodes.NetworkError:
                    errorEventArgs.Error = GoogleClientErrorType.SignInNetworkError;
                    errorEventArgs.Message = GoogleClientBaseException.SignInNetworkErrorMessage;
                    exception = new GoogleClientSignInNetworkErrorException();
                    break;
                case CommonStatusCodes.InvalidAccount:
                    errorEventArgs.Error = GoogleClientErrorType.SignInInvalidAccountError;
                    errorEventArgs.Message = GoogleClientBaseException.SignInInvalidAccountErrorMessage;
                    exception = new GoogleClientSignInInvalidAccountErrorException();
                    break;
                case CommonStatusCodes.SignInRequired:
                    errorEventArgs.Error = GoogleClientErrorType.SignInRequiredError;
                    errorEventArgs.Message = GoogleClientBaseException.SignInRequiredErrorMessage;
                    exception = new GoogleClientSignInRequiredErrorErrorException();
                    break;
                case GoogleSignInStatusCodes.SignInFailed:
                    errorEventArgs.Error = GoogleClientErrorType.SignInFailedError;
                    errorEventArgs.Message = GoogleClientBaseException.SignInFailedErrorMessage;
                    exception = new GoogleClientSignInFailedErrorException();
                    break;
                case GoogleSignInStatusCodes.SignInCancelled:
                    errorEventArgs.Error = GoogleClientErrorType.SignInCanceledError;
                    errorEventArgs.Message = GoogleClientBaseException.SignInCanceledErrorMessage;
                    exception = new GoogleClientSignInCanceledErrorException();
                    break;
                default:
                    errorEventArgs.Error = GoogleClientErrorType.SignInDefaultError;
                    errorEventArgs.Message = apiException.Message;
                    exception = new GoogleClientBaseException(
                        string.IsNullOrEmpty(apiException.Message)
                            ? GoogleClientBaseException.SignInDefaultErrorMessage
                            : apiException.Message
                        );
                    break;
            }

            loginTcs.TrySetException(exception);
        }


        public void OnComplete(Android.Gms.Tasks.Task task) {
            if (!task.IsSuccessful) {
                OnSignInFailed(task.Exception.JavaCast<ApiException>());
            } else {
                var userAccount = task.Result.JavaCast<GoogleSignInAccount>();

                OnSignInSuccessful(userAccount);
            }
        }
    }
}
