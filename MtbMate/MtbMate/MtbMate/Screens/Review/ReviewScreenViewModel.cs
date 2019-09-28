using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Controls;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MtbMate.Screens.Review {
    public class ReviewScreenViewModel : ViewModelBase {
        public readonly Ride Ride;

        public ReviewScreenViewModel(MainContext context, Ride ride) : base(context) {
            Ride = ride;
            MapViewModel = new MapControlViewModel(
                context,
                Ride.DisplayName,
                PolyUtils.GetMapLocations(Ride.Locations));
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

        public string MaxGForce => 0 + "g"; // temp

        public IList<SegmentAttempt> Attempts => Model.Instance.SegmentAttempts
            .Where(i => i.RideId == Ride.Id)
            .OrderByDescending(i => i.Created)
            .ToList();

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

        public async Task Export() {
            await Share.RequestAsync(new ShareFileRequest {
                File = Ride.GetReadingsFile(),
                Title = Ride.Name ?? "Data Readings",
            });
        }

        public async Task ExportLocation() {
            await Share.RequestAsync(new ShareFileRequest {
                File = Ride.GetLocationFile(),
                Title = Ride.Name ?? "Data Readings",
            });
        }

        public async Task GoToAttempt(INavigation nav, SegmentAttempt attempt) {
            await Context.UI.GoToSegmentAttemptScreenAsync(nav, attempt);
        }
    }
}
