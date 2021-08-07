using System.Collections.Generic;
using System.Linq;
using Microcharts;
using Shared.Dtos;
using Tracked.Contexts;

namespace Tracked.Screens.Rides {
    public class SpeedAnalysisScreenViewModel : ViewModelBase {
        private readonly IList<RideLocationDto> rideLocations;

        public Chart Chart { get; }

        public SpeedAnalysisScreenViewModel(MainContext context, IList<RideLocationDto> rideLocations) : base(context) {
            this.rideLocations = rideLocations;

            Chart = CreateAnaysisChartModel();
        }

        public override string Title => "Speed & Altitude";

        private Chart CreateAnaysisChartModel() {
            return new LineChart {
                Entries = rideLocations
                    .Select(i => new ChartEntry((float)i.Mph)),
            };
        }
    }
}