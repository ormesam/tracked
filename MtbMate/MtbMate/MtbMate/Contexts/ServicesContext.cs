using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MtbMate.Models;
using MtbMate.Utilities;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos;
using Xamarin.Essentials;
using Location = MtbMate.Models.Location;

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

        public async Task<int?> Sync(Ride ride) {
            int? rideId = await PostAsync<int?>("rides/add", new RideDto {
                RideId = ride.RideId,
                ClientId = ride.Id.Value,
                End = ride.End,
                Start = ride.Start,
                Jumps = ride.Jumps
                    .Select(i => new JumpDto {
                        Airtime = Convert.ToDecimal(i.Airtime),
                        Number = i.Number,
                        Timestamp = i.Timestamp,
                    })
                    .ToList(),
                Locations = ride.Locations
                    .Select(i => new RideLocationDto {
                        AccuracyInMetres = Convert.ToDecimal(i.AccuracyInMetres),
                        Altitude = Convert.ToDecimal(i.Altitude),
                        Latitude = Convert.ToDecimal(i.Point.Latitude),
                        Longitude = Convert.ToDecimal(i.Point.Longitude),
                        SpeedMetresPerSecond = Convert.ToDecimal(i.SpeedMetresPerSecond),
                        Timestamp = i.Timestamp,
                    })
                    .ToList(),
                AccelerometerReadings = ride.AccelerometerReadings
                    .Select(i => new AccelerometerReadingDto {
                        Time = i.Timestamp,
                        X = Convert.ToDecimal(i.X),
                        Y = Convert.ToDecimal(i.Y),
                        Z = Convert.ToDecimal(i.Z),
                    })
                    .ToList(),
            });

            return rideId;
        }

        public async Task<int?> Sync(Segment segment) {
            int? segmentId = await PostAsync<int?>("segments/add", new SegmentDto {
                SegmentId = segment.SegmentId,
                Name = segment.Name,
                Locations = segment.Points
                    .Select(i => new SegmentLocationDto {
                        SegmentId = segment.SegmentId,
                        Order = i.Order,
                        Latitude = Convert.ToDecimal(i.Point.Latitude),
                        Longitude = Convert.ToDecimal(i.Point.Longitude),
                    })
                    .ToList(),
            });

            return segmentId;
        }

        public async Task<int?> Sync(SegmentAttempt segmentAttempt) {
            int? segmentAttemptId = await PostAsync<int?>("segments/add-attempt", new SegmentAttemptDto {
                SegmentAttemptId = segmentAttempt.SegmentAttemptId,
                SegmentId = segmentAttempt.Segment.SegmentId.Value,
                StartUtc = segmentAttempt.Start,
                EndUtc = segmentAttempt.End,
                Medal = segmentAttempt.Medal,
                RideId = segmentAttempt.Ride.RideId.Value,
            });

            return segmentAttemptId;
        }

        public async Task<IList<Ride>> GetRides(IList<int> existingRideIds) {
            var results = await PostAsync<IList<RideDto>>("rides/get", existingRideIds);

            return results
                .Select(ride => new Ride {
                    RideId = ride.RideId,
                    Id = ride.ClientId,
                    End = ride.End,
                    Start = ride.Start,
                    Jumps = ride.Jumps
                        .Select(i => new Jump {
                            Airtime = Convert.ToDouble(i.Airtime),
                            Number = i.Number,
                            Timestamp = i.Timestamp,
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
                    AccelerometerReadings = ride.AccelerometerReadings
                        .Select(i => new AccelerometerReading {
                            Timestamp = i.Time,
                            X = Convert.ToDouble(i.X),
                            Y = Convert.ToDouble(i.Y),
                            Z = Convert.ToDouble(i.Z),
                        })
                        .ToList(),
                })
                .ToList();
        }

        public async Task<IList<Segment>> GetSegments(IList<int> existingSegmentIds) {
            var results = await PostAsync<IList<SegmentDto>>("segments/get", existingSegmentIds);

            return results
                .Select(s => new Segment {
                    SegmentId = s.SegmentId,
                    Name = s.Name,
                    Points = s.Locations
                        .Select(l => new SegmentLocation(l.Order, Convert.ToDouble(l.Latitude), Convert.ToDouble(l.Longitude)))
                        .ToList(),
                })
                .ToList();
        }

        public async Task<IList<SegmentAttempt>> GetSegmentAttempts(IList<int> existingAttemptIds) {
            var results = await PostAsync<IList<SegmentAttemptDto>>("segments/get-attempts", existingAttemptIds);

            var ridesById = Model.Instance.Rides
                .Where(row => row.RideId != null)
                .ToDictionary(row => row.RideId.Value, row => row.Id.Value);

            var segmentsById = Model.Instance.Segments
                .Where(row => row.SegmentId != null)
                .ToDictionary(row => row.SegmentId, row => row.Id.Value);

            return results
                .Select(s => new SegmentAttempt {
                    SegmentAttemptId = s.SegmentAttemptId,
                    Start = s.StartUtc,
                    End = s.EndUtc,
                    Medal = s.Medal,
                    SegmentId = segmentsById[s.SegmentId.Value],
                    RideId = ridesById[s.RideId.Value],
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
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) {
                Toast.LongAlert("Cannot connect to the internet...");

                return default;
            }

            using (HttpClient client = new HttpClient()) {
                client.BaseAddress = baseUri;
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
                await mainContext.Security.ClearAccessToken();

                throw new ServiceException("You have been logged out.");
            }

            throw new ServiceException("API Error");
        }
    }
}