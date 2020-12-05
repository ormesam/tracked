using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
    [Authorize]
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
            GoogleResponse googleResponse = await GetGoogleDetails(loginDto.GoogleIdToken);

            if (string.IsNullOrWhiteSpace(googleResponse?.GoogleUserId)) {
                return Unauthorized();
            }

            var user = context.User.SingleOrDefault(row => row.GoogleUserId == googleResponse.GoogleUserId);

            if (user == null) {
                user = new User {
                    GoogleUserId = googleResponse.GoogleUserId,
                    Name = loginDto.User.Name,
                    CreatedUtc = DateTime.UtcNow,
                    ProfileImageUrl = loginDto.User.Picture.ToString(),
                };

                context.User.Add(user);

                context.SaveChanges();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserId.ToString()),
                new Claim("IsAdmin", user.IsAdmin.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: "samorme.com",
                audience: "samorme.com",
                expires: DateTime.UtcNow.AddSeconds(30),
                claims: claims,
                signingCredentials: credentials);

            return new LoginResponseDto {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                User = GetUser(user.UserId),
            };
        }

        private UserDto GetUser(int userId) {
            return context.User
                .Where(row => row.UserId == userId)
                .Select(row => new UserDto {
                    UserId = row.UserId,
                    Name = row.Name,
                    ProfileImageUrl = row.ProfileImageUrl,
                    IsAdmin = row.IsAdmin,
                    CreatedUtc = row.CreatedUtc,
                })
                .Single();
        }

        private async Task<GoogleResponse> GetGoogleDetails(string idToken) {
            try {
                var message = new HttpRequestMessage(HttpMethod.Post, "/oauth2/v3/tokeninfo");
                message.Content = new StringContent(JsonConvert.SerializeObject(new { id_token = idToken }));

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
        }
    }
}