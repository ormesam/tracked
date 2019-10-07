using System.Collections.Generic;
using MtbMate.Contexts;
using MtbMate.Controls;
using MtbMate.Models;
using MtbMate.Utilities;

namespace MtbMate.Screens.Segments {
    public class AttemptScreenViewModel : ViewModelBase {
        private readonly SegmentAttempt attempt;

        public MapControlViewModel MapViewModel { get; }

        public AttemptScreenViewModel(MainContext context, SegmentAttempt attempt) : base(context) {
            this.attempt = attempt;

            MapViewModel = new MapControlViewModel(
                context,
                attempt.DisplayName,
                PolyUtils.GetMapLocations(attempt));
        }

        public override string Title => DisplayName;

        public IList<Location> Locations => attempt.Locations;
        public IList<Jump> Jumps => attempt.Jumps;
        public string DisplayName => attempt.Segment.DisplayName;
        public double AverageSpeed => attempt.AverageSpeed;
        public double MaxSpeed => attempt.MaxSpeed;
        public double Distance => attempt.Distance;
        public string Time => attempt.FormattedTime;
    }
}
