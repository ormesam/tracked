using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shared.Dtos;

namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase {
        private readonly IOptions<AppSettings> appSettings;
        private ModelDataContext context;

        public LoginController(IOptions<AppSettings> appSettings, ModelDataContext context) {
            this.appSettings = appSettings;
            this.context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<ActionResult<LoginResponseDto>> Authenticate(LoginDto loginDto) {
            GoogleResponse googleResponse = await GetGoogleDetails(loginDto.GoogleAccessToken);

            if (string.IsNullOrWhiteSpace(googleResponse?.GoogleUserId)) {
                return Unauthorized();
            }

            var user = context.User.SingleOrDefault(row => row.GoogleUserId == googleResponse.GoogleUserId);

            if (user == null) {
                user = new User {
                    GoogleUserId = googleResponse.GoogleUserId,
                    Name = googleResponse.Name
                };

                context.User.Add(user);

                context.SaveChanges();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "samorme.com",
                audience: "samorme.com",
                signingCredentials: credentials);

            return new LoginResponseDto {
                Name = googleResponse.Name,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            };
        }

        private async Task<GoogleResponse> GetGoogleDetails(string accessToken) {
            try {
                var message = new HttpRequestMessage(HttpMethod.Post, "/oauth2/v3/tokeninfo");
                message.Content = new StringContent(JsonConvert.SerializeObject(new { id_token = accessToken }));

                using (HttpClient client = new HttpClient()) {
                    client.BaseAddress = new Uri("https://www.googleapis.com");

                    var response = await client.SendAsync(message);

                    if (!response.IsSuccessStatusCode) {
                        return null;
                    }

                    var result = JsonConvert.DeserializeObject<GoogleResponse>(await response.Content.ReadAsStringAsync());

                    return result;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex);

                return null;
            }
        }

        private class GoogleResponse {
            [JsonProperty("sub")]
            public string GoogleUserId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("aud")]
            public string AppId { get; set; } // TODO
        }
    }
}