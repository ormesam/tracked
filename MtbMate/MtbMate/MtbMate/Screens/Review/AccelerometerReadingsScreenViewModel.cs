using System;
using System.Linq;
using MtbMate.Contexts;
using MtbMate.Models;
using OxyPlot.Axes;
using OxyPlot.Series;
using ChartPlotModel = OxyPlot.PlotModel;

namespace MtbMate.Screens.Review {
    internal class AccelerometerReadingsScreenViewModel : ViewModelBase {
        private readonly IRide ride;

        public ChartPlotModel AccelerometerChartModel { get; }

        public AccelerometerReadingsScreenViewModel(MainContext context, IRide ride)
            : base(context) {
            this.ride = ride;

            AccelerometerChartModel = CreateAccelerometerChartModel();
        }

        public override string Title => ride.DisplayName;

        private ChartPlotModel CreateAccelerometerChartModel() {
            var readingTimestamps = ride.AccelerometerReadings
                .Select(i => i.Timestamp)
                .ToList();

            var plot = new ChartPlotModel {
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
                          ItemsSource = ride.AccelerometerReadings
                            .Select(i => new {
                                x = readingTimestamps.IndexOf(i.Timestamp),
                                y = Math.Abs(i.SmoothedValue),
                            })
                            .ToList(),
                          DataFieldX = "x",
                          DataFieldY = "y",
                    },
                }
            };

            foreach (var jump in ride.Jumps) {
                plot.Series.Add(new LineSeries {
                    ItemsSource = jump.Readings
                        .Select(i => new {
                            x = readingTimestamps.IndexOf(i.Timestamp),
                            y = Math.Abs(i.SmoothedValue),
                        })
                        .ToList(),
                    DataFieldX = "x",
                    DataFieldY = "y",
                    Color = OxyPlot.OxyColor.FromRgb(255, 0, 0),
                });
            }

            return plot;
        }
    }
}