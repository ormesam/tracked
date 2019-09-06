using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Screens.Segments
{
    public class AttemptScreenViewModel : ViewModelBase
    {
        private readonly SegmentAttemptModel attempt;

        public AttemptScreenViewModel(MainContext context, SegmentAttemptModel attempt) : base(context) {
            this.attempt = attempt;
        }

        public RideModel Ride => Model.Instance.Rides
            .Where(i => i.Id == attempt.RideId)
            .Single();

        public SegmentModel Segment => Model.Instance.Segments
            .Where(i => i.Id == attempt.SegmentId)
            .Single();

        public IList<LocationModel> Locations => Ride.MovingLocations
            .ToList()
            .GetRange(attempt.StartIdx, (attempt.EndIdx - attempt.StartIdx) + 1);


        public override string Title => DisplayName;

        public string DisplayName => Segment.DisplayName;

        public double AverageSpeed {
            get {
                if (!Locations.Any()) {
                    return 0;
                }

                return Locations.Average(i => i.Mph);
            }
        }

        public double MaxSpeed {
            get {
                if (!Locations.Any()) {
                    return 0;
                }

                return Locations.Max(i => i.Mph);
            }
        }

        public double Distance {
            get {
                if (!Ride.Locations.Any()) {
                    return 0;
                }

                return Locations.CalculateDistanceMi();
            }
        }

        public string Time => (Locations.Last().Timestamp - Locations.First().Timestamp).ToString(@"mm\:ss");

        public async Task GoToMapScreen(INavigation nav) {
            await Context.UI.GoToMapScreenAsync(nav, DisplayName, Locations);
        }
    }
}
