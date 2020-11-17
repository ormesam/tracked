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

        public async Task<LoginResponseDto> Login(string idToken, GoogleUserDto user) {
            return await PostAsync<LoginResponseDto>("login/authenticate", new LoginDto {
                GoogleIdToken = idToken,
                User = user,
            });
        }

        public async Task<IList<RideOverviewDto>> GetRideOverviews() {
            return await GetAsync<IList<RideOverviewDto>>("rides");
        }

        public async Task<RideDto> GetRide(int id) {
            return await GetAsync<RideDto>("rides/" + id);
        }

        public async Task<RideOverviewDto> UploadRide(CreateRideDto ride) {
            return await PostAsync<RideOverviewDto>("rides/add", ride);
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

        public async Task<IList<AchievementDto>> GetAchievements() {
            return await GetAsync<IList<AchievementDto>>("achievements");
        }

        protected async Task<TResult> GetAsync<TResult>(string apiEndpoint) {
            return await SendAsync<TResult>(CreateGetRequestMessage(apiEndpoint));
        }

        private HttpRequestMessage CreateGetRequestMessage(string apiEndpoint) {
            var message = new HttpRequestMessage(HttpMethod.Get, Constants.Url + "/api/" + apiEndpoint);

            return message;
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
                        await HandleRequestError(response);
                    }

                    var jsonSerializerSettings = new JsonSerializerSettings {
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    };

                    return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync(), jsonSerializerSettings);
                } catch (Exception ex) {
                    throw new ServiceException(ex.Message);
                }
            }
        }

        private async Task HandleRequestError(HttpResponseMessage response) {
            if (response.StatusCode == HttpStatusCode.Unauthorized) {
                await mainContext.Security.Logout();

                throw new ServiceException("You have been logged out.");
            }

            throw new ServiceException("API Error");
        }
    }
}