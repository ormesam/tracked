using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared;
using Shared.Dto;
using Shared.Dtos;
using Tracked.Utilities;
using Xamarin.Essentials;

namespace Tracked.Contexts {
    public class ServicesContext {
        private readonly MainContext mainContext;

        public ServicesContext(MainContext mainContext) {
            this.mainContext = mainContext;
        }

        public async Task<LoginResponseDto> Authenticate(string idToken, GoogleUserDto user) {
            return await PostAsync<LoginResponseDto>("login/authenticate", new LoginDto {
                GoogleIdToken = idToken,
                User = user,
            });
        }

        public async Task<LoginResponseDto> Login(string refreshToken) {
            return await PostAsync<LoginResponseDto>("login/token", new RefreshTokenDto {
                RefreshToken = refreshToken,
            });
        }

        public async Task<FeedWrapperDto> GetFeed() {
            return await GetAsync<FeedWrapperDto>("feed");
        }

        public async Task<IList<UserSearchDto>> SearchUsers(string searchText) {
            return await GetAsync<IList<UserSearchDto>>("search?searchText=" + searchText);
        }

        public async Task<RideDto> GetRide(int id) {
            return await GetAsync<RideDto>("rides/" + id);
        }

        public async Task<RideFeedDto> SaveRide(CreateRideDto ride) {
            return await PostAsync<RideFeedDto>("rides/add", ride);
        }

        public async Task ReanalyseRide(int rideId) {
            await PostAsync("rides/reanalyse", rideId);
        }

        public async Task DeleteRide(int rideId) {
            await PostAsync<bool>("rides/delete", rideId);
        }

        public async Task<IList<TrailOverviewDto>> GetTrailOverviews() {
            return await GetAsync<IList<TrailOverviewDto>>("trails");
        }

        public async Task<int> UploadTrail(TrailDto trail) {
            return await PostAsync<int>("trails/add", trail);
        }

        public async Task<string> ChangeTrailName(int trailId, string newName) {
            return await PostAsync<string>("trails/change-name", new TrailChangeNameDto {
                TrailId = trailId,
                Name = newName,
            });
        }

        public async Task<TrailDto> GetTrail(int id) {
            return await GetAsync<TrailDto>("trails/" + id);
        }

        public async Task DeleteTrail(int trailId) {
            await PostAsync<bool>("trails/delete", trailId);
        }

        public async Task<ProfileDto> GetProfile() {
            return await GetAsync<ProfileDto>("users/profile");
        }

        public async Task UpdateBio(string bio) {
            await PostAsync("users/update-bio", new BioChangeDto {
                Bio = bio,
            });
        }

        public async Task<int> GetLatestAnalyserVersion() {
            return await GetAsync<int>("rides/latest-analyser-version");
        }

        protected async Task<TResult> GetAsync<TResult>(string apiEndpoint) {
            return await SendAsync<TResult>(CreateGetRequestMessage(apiEndpoint));
        }

        private HttpRequestMessage CreateGetRequestMessage(string apiEndpoint) {
            var message = new HttpRequestMessage(HttpMethod.Get, Constants.Url + "/api/" + apiEndpoint);

            return message;
        }

        protected async Task PostAsync(string apiEndpoint, object data = null) {
            await SendAsync<object>(CreatePostRequestMessage(apiEndpoint, data));
        }

        protected async Task<TResult> PostAsync<TResult>(string apiEndpoint, object data = null) {
            return await SendAsync<TResult>(CreatePostRequestMessage(apiEndpoint, data));
        }

        private HttpRequestMessage CreatePostRequestMessage(string apiEndpoint, object data) {
            var message = new HttpRequestMessage(HttpMethod.Post, Constants.Url + "/api/" + apiEndpoint);

            if (data != null) {
                string json = JsonConvert.SerializeObject(data);

                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return message;
        }

        private async Task<TResult> SendAsync<TResult>(HttpRequestMessage request) {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) {
                Toast.LongAlert("Cannot connect to the internet...");

                return default;
            }

            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrWhiteSpace(mainContext.Security.AccessToken)) {
                    request.Headers.Add("Authorization", "Bearer " + mainContext.Security.AccessToken);
                }

                try {
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode) {
                        throw HandleRequestError(response);
                    }

                    var jsonSerializerSettings = new JsonSerializerSettings {
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    };

                    return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync(), jsonSerializerSettings);
                } catch (ServiceException) {
                    throw;
                } catch (Exception) {
                    throw new ServiceException("Unable to connect to server.\nMake sure you are connected to the internet and try again.", ServiceExceptionType.UnableToConnect);
                }
            }
        }

        private ServiceException HandleRequestError(HttpResponseMessage response) {
            switch (response.StatusCode) {
                case HttpStatusCode.InternalServerError:
                    return new ServiceException("Something went wrong. It's not you, it's us.", ServiceExceptionType.ServerError);
                case HttpStatusCode.BadRequest:
                    return new ServiceException("Unable to complete operaton.", ServiceExceptionType.BadRequest);
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Unauthorized:
                    mainContext.Security.Logout();

                    return new ServiceException("You have been logged out.", ServiceExceptionType.Unauthorized);
                case HttpStatusCode.NotFound:
                    return new ServiceException("Not found.", ServiceExceptionType.NotFound);
                default:
                    return new ServiceException("An error occured. Please try again later.", ServiceExceptionType.Unknown);
            }
        }
    }
}