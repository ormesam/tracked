﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MtbMate.Models;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos;

namespace MtbMate.Contexts {
    public class ServicesContext {
        private readonly MainContext mainContext;
        private readonly Uri baseUri = new Uri(Constants.Url);

        public ServicesContext(MainContext mainContext) {
            this.mainContext = mainContext;
        }

        public async Task<LoginResponseDto> Login(string googleAccessToken) {
            return await PostAsync<LoginResponseDto>("login/authenticate", new LoginDto {
                GoogleAccessToken = googleAccessToken,
            });
        }

        public async Task<int> Sync(Ride ride) {
            int rideId = await PostAsync<int>("rides/add", new RideDto {
                ClientId = ride.Id.Value,
                End = ride.End.Value,
                Start = ride.Start.Value,
                Name = ride.Name,
                RideId = ride.RideId,
                Jumps = ride.Jumps
                    .Select(i => new JumpDto {
                        Airtime = Convert.ToDecimal(i.Airtime),
                        LandingGForce = Convert.ToDecimal(i.LandingGForce),
                        Number = i.Number,
                        Time = i.Time,
                    })
                    .ToList(),
                Locations = ride.Locations
                    .Select(i => new LocationDto {
                        AccuracyInMetres = Convert.ToDecimal(i.AccuracyInMetres),
                        Altitude = Convert.ToDecimal(i.Altitude),
                        Latitude = Convert.ToDecimal(i.Point.Latitude),
                        Longitude = Convert.ToDecimal(i.Point.Longitude),
                        SpeedMetresPerSecond = Convert.ToDecimal(i.SpeedMetresPerSecond),
                        Timestamp = i.Timestamp,
                    })
                    .ToList(),
            });

            return rideId;
        }

        public async Task<IList<Ride>> GetRides(List<int> existingRideIds) {
            var results = await PostAsync<IList<RideDto>>("rides/get", existingRideIds);

            return results
                .Select(ride => new Ride {
                    Id = ride.ClientId,
                    End = ride.End,
                    Start = ride.Start,
                    Name = ride.Name,
                    RideId = ride.RideId,
                    Jumps = ride.Jumps
                        .Select(i => new Jump {
                            Airtime = Convert.ToDouble(i.Airtime),
                            LandingGForce = Convert.ToDouble(i.LandingGForce),
                            Number = i.Number,
                            Time = i.Time,
                        })
                        .ToList(),
                    Locations = ride.Locations
                        .Select(i => new Location {
                            AccuracyInMetres = Convert.ToDouble(i.AccuracyInMetres),
                            Altitude = Convert.ToDouble(i.Altitude),
                            Point = new LatLng(Convert.ToDouble(i.Latitude), Convert.ToDouble(i.Longitude)),
                            SpeedMetresPerSecond = Convert.ToDouble(i.SpeedMetresPerSecond),
                            Timestamp = i.Timestamp,
                        })
                        .ToList(),
                })
                .ToList();
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

                var jsonSerializerSettings = new JsonSerializerSettings {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                };

                return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync(), jsonSerializerSettings);
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