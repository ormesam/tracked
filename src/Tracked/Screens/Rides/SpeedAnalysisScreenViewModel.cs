using System.Collections.Generic;
using System.Linq;
using Microcharts;
using Shared.Dtos;
using Tracked.Contexts;

namespace Tracked.Screens.Rides {
    public class SpeedAnalysisScreenViewModel : ViewModelBase {
        private readonly IList<RideLocationDto> rideLocations;

        public Chart SpeedChart { get; }
        public Chart AltitudeChart { get; }

        public SpeedAnalysisScreenViewModel(MainContext context, IList<RideLocationDto> rideLocations) : base(context) {
            this.rideLocations = rideLocations;

            SpeedChart = CreateSpeedChart();
            AltitudeChart = CreateAltitudeChart();
        }

        public override string Title => "Speed & Altitude";

        private Chart CreateSpeedChart() {
            return new LineChart {
                Entries = rideLocations
                    .Select(i => new ChartEntry((float)i.Mph)),
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.None,
                PointSize = 0,
                MinValue = 0,
            };
        }

        private Chart CreateAltitudeChart() {
            return new LineChart {
                Entries = rideLocations
                    .Select(i => new ChartEntry((float)i.Altitude)),
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.None,
                PointSize = 0,
                MinValue = (float)rideLocations.Min(i => i.Altitude),
            };
        }
    }
}