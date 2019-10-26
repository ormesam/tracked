﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Controls;
using MtbMate.Models;
using MtbMate.Utilities;
using OxyPlot.Axes;
using OxyPlot.Series;
using Xamarin.Essentials;
using Xamarin.Forms;
using ChartPlotModel = OxyPlot.PlotModel;

namespace MtbMate.Screens.Review {
    public class RideReviewScreenViewModel : ViewModelBase {
        public readonly Ride Ride;
        public ChartPlotModel AnalysisChartModel { get; }
        public ChartPlotModel AccelerometerChartModel { get; }

        public RideReviewScreenViewModel(MainContext context, Ride ride) : base(context) {
            Ride = ride;

            MapViewModel = new MapControlViewModel(
                context,
                Ride.DisplayName,
                PolyUtils.GetMapLocations(Ride));

            AnalysisChartModel = CreateAnaysisChartModel();
            AccelerometerChartModel = CreateAccelerometerChartModel();
        }

        public override string Title => DisplayName;

        public string DisplayName => Ride.DisplayName;

        public MapControlViewModel MapViewModel { get; }

        public double AverageSpeed {
            get {
                if (!Ride.Locations.Any()) {
                    return 0;
                }

                return Ride.Locations.Average(i => i.Mph);
            }
        }

        public double MaxSpeed {
            get {
                if (!Ride.Locations.Any()) {
                    return 0;
                }

                return Ride.Locations.Max(i => i.Mph);
            }
        }

        public double Distance {
            get {
                if (!Ride.Locations.Any()) {
                    return 0;
                }

                return Ride.Locations.CalculateDistanceMi();
            }
        }

        public string Time => (Ride.End.Value - Ride.Start.Value).ToString(@"mm\:ss");

        public int JumpCount => Ride.Jumps.Count;
        public int SegmentCount => Attempts.Count;
        public string MaxGForce => Ride.Jumps.Count == 0 ? "-" : $"{Ride.Jumps.Max(i => i.LandingGForce)}g";
        public string MaxAirtime => Ride.Jumps.Count == 0 ? "-" : $"{Ride.Jumps.Max(i => i.Airtime)}s";

        public IList<SegmentAttempt> Attempts => Model.Instance.SegmentAttempts
            .Where(i => i.RideId == Ride.Id)
            .OrderByDescending(i => i.Created)
            .ToList();

        public IList<Jump> Jumps => Ride.Jumps
            .OrderBy(i => i.Time)
            .ToList();

        private ChartPlotModel CreateAnaysisChartModel() {
            int count1 = 0;
            int count2 = 0;
            string speedKey = "Speed";
            string altitudeKey = "Altitude";

            return new ChartPlotModel {
                Title = "Speed & Altitude",
                Axes = {
                    new CategoryAxis {
                        Position = AxisPosition.Bottom,
                    },
                    new LinearAxis {
                        Key=speedKey,
                        Position = AxisPosition.Left,
                        MinimumPadding = 0,
                    },
                    new LinearAxis {
                        Key=altitudeKey,
                        Position = AxisPosition.Right,
                        MinimumPadding = 0,
                    },
                },
                Series = {
                    new LineSeries()
                    {
                          ItemsSource = Ride.Locations
                            .Select(i => new {
                                x = count1++,
                                y = i.Mph,
                            })
                            .ToList(),
                          DataFieldX = "x",
                          DataFieldY = "y",
                          YAxisKey = speedKey,
                    },
                    new LineSeries()
                    {
                          ItemsSource = Ride.Locations
                            .Select(i => new {
                                x = count2++,
                                y = i.Altitude,
                            })
                            .ToList(),
                          DataFieldX = "x",
                          DataFieldY = "y",
                          YAxisKey = altitudeKey,
                    },
                }
            };
        }

        private ChartPlotModel CreateAccelerometerChartModel() {
            int count = 0;

            return new ChartPlotModel {
                Title = "Accelerometer",
                Axes = {
                    new CategoryAxis {
                        Position = AxisPosition.Bottom,
                    },
                    new LinearAxis {
                        Position = AxisPosition.Left,
                        MinimumPadding = 0,
                    },
                },
                Series = {
                    new LineSeries()
                    {
                          ItemsSource = Ride.AccelerometerReadings
                            .Select(i => new {
                                x = count++,
                                y = i.SmoothedValue,
                            })
                            .ToList(),
                          DataFieldX = "x",
                          DataFieldY = "y",
                    },
                }
            };
        }

        public async Task Delete() {
            await Model.Instance.RemoveRide(Ride);
        }

        public void ChangeName() {
            Context.UI.ShowInputDialog("Change Name", Ride.Name, async (newName) => {
                Ride.Name = newName;

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DisplayName));

                await Model.Instance.SaveRide(Ride);
            });
        }

        public async Task GoToAttempt(INavigation nav, SegmentAttempt attempt) {
            await Context.UI.GoToSegmentAttemptScreenAsync(nav, attempt);
        }

        public async Task ExportJumpData() {
            await Share.RequestAsync(new ShareFileRequest {
                File = Ride.GetReadingsFile(),
                Title = Ride.Name ?? "Data Readings",
            });
        }
    }
}
