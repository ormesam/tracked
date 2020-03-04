using System.Linq;
using Tracked.Contexts;
using Tracked.Models;
using OxyPlot.Axes;
using OxyPlot.Series;
using ChartPlotModel = OxyPlot.PlotModel;

namespace Tracked.Screens.Review {
    public class SpeedAnalysisScreenViewModel : ViewModelBase {
        private readonly IRide ride;
        public ChartPlotModel AnalysisChartModel { get; }

        public SpeedAnalysisScreenViewModel(MainContext context, IRide ride) : base(context) {
            this.ride = ride;

            AnalysisChartModel = CreateAnaysisChartModel();
        }

        public override string Title => "Speed & Altitude";

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
                          ItemsSource = ride.Locations
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
                          ItemsSource = ride.Locations
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
    }
}