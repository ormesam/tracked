using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos;

namespace MtbMate.Contexts {
    public class ServicesContext {
        private MainContext mainContext;
        private readonly Uri baseUri = new Uri(Constants.Url);

        public ServicesContext(MainContext mainContext) {
            this.mainContext = mainContext;
        }

        public async Task<LoginResponseDto> Login(string googleAccessToken) {
            return await PostAsync<LoginResponseDto>("login/authenticate", new LoginDto {
                GoogleAccessToken = googleAccessToken,
            });
        }

        protected async Task<TResult> PostAsync<TResult>(string apiEndpoint, object data = null) {
            return await SendAsync<TResult>(CreatePostRequestMessage(apiEndpoint, data));
        }

        private HttpRequestMessage CreatePostRequestMessage(string apiEndpoint, object data) {
            var message = new HttpRequestMessage(HttpMethod.Post, "/api/" + apiEndpoint);

            if (data != null) {
                string json = JsonConvert.SerializeObject(data);

                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return message;
        }

        private async Task<TResult> SendAsync<TResult>(HttpRequestMessage request) {
            using (HttpClient client = new HttpClient()) {
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrWhiteSpace(mainContext.Security.AccessToken)) {
                    request.Headers.Add("Authorization", "Bearer " + mainContext.Security.AccessToken);
                }

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode) {
                    await HandleRequestError(response);
                }

                return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
            }
        }

        private async Task HandleRequestError(HttpResponseMessage response) {
            if (response.StatusCode == HttpStatusCode.Unauthorized) {
                await mainContext.Security.ClearAccessToken();

                throw new Exception("You have been logged out.");
            }

            throw new Exception("API Error");
        }
    }
}