using System;
using System.Linq;
using MtbMate.Contexts;
using MtbMate.Controls;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MtbMate.Screens.Segments {
    public class CreateSegmentScreenViewModel : ViewModelBase {
        private readonly Segment segment;
        private int count;
        private LatLng lastLatLng;
        private string displayText;

        public Ride Ride { get; }
        public MapControlViewModel MapViewModel { get; }

        public CreateSegmentScreenViewModel(MainContext context, Ride ride) : base(context) {
            segment = new Segment();
            Ride = ride;
            count = 1;
            displayText = "Tap on the map to set a start point";

            MapViewModel = new MapControlViewModel(
                context,
                Ride.DisplayName,
                PolyUtils.GetMapLocations(Ride),
                isReadOnly: false,
                showRideFeatures: false,
                isShowingUser: false,
                goToMapPageOnClick: false,
                mapType: MapType.Satellite,
                canChangeMapType: true);

            MapViewModel.MapTapped += MapViewModel_MapTapped;
        }

        private void MapViewModel_MapTapped(object sender, MapClickedEventArgs e) {
            AddPin(e.Point.Latitude, e.Point.Longitude);
        }

        public string Name {
            get { return segment.Name; }
            set { segment.Name = value; }
        }

        public string DisplayText {
            get { return displayText; }
            set {
                if (displayText != value) {
                    displayText = value;
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

        public override string Title => "Create Segment";

        public void Save(INavigation nav) {
            if (!segment.Points.Any()) {
                return;
            }

            Context.UI.ShowInputDialog("Segment Name", string.Empty, async (newName) => {
                if (string.IsNullOrWhiteSpace(newName)) {
                    return;
                }

                segment.Name = newName;
                segment.Created = DateTime.UtcNow;

                await Model.Instance.SaveSegment(segment);

                await nav.PopAsync();
            });
        }

        public void AddPin(double latitude, double longitude) {
            segment.Points.Add(new SegmentLocation(count++, latitude, longitude));

            LatLng thisLatLng = new LatLng(latitude, longitude);

            if (lastLatLng != null) {
                MapViewModel.AddPolyLine(new[] { lastLatLng, thisLatLng }, Color.Red);
            } else {
                DisplayText = "Now tap to set the next point";
            }

            lastLatLng = thisLatLng;
        }
    }
}
