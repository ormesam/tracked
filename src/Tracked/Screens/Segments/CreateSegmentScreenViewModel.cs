using System.Collections.Generic;
using Shared.Dtos;
using Shared.Interfaces;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Tracked.Screens.Segments {
    public class CreateSegmentScreenViewModel : ViewModelBase {
        private readonly SegmentDto segment;
        private int count;
        private ILatLng lastLatLng;
        private string displayText;

        public Ride Ride { get; }
        public MapControlViewModel MapViewModel { get; }

        public CreateSegmentScreenViewModel(MainContext context, Ride ride) : base(context) {
            segment = new SegmentDto();
            Ride = ride;
            count = 1;
            displayText = "Tap on the map to set a start point";

            MapViewModel = new MapControlViewModel(
                context,
                Ride?.DisplayName ?? "Map",
                /*ride != null ? PolyUtils.GetMapLocations(Ride) :*/ new List<MapLocation>(),
                isReadOnly: false,
                showRideFeatures: false,
                isShowingUser: false,
                goToMapPageOnClick: false,
                mapType: MapType.Satellite,
                canChangeMapType: true);

            MapViewModel.MapTapped += MapViewModel_MapTapped;
        }

        private void MapViewModel_MapTapped(object sender, MapClickedEventArgs e) {
            AddPin((decimal)e.Point.Latitude, (decimal)e.Point.Longitude);
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
            if (segment.Locations.Count <= 2) {
                return;
            }

            Context.UI.ShowInputDialog("Segment Name", string.Empty, async (newName) => {
                if (string.IsNullOrWhiteSpace(newName)) {
                    return;
                }

                segment.Name = newName;

                await Context.Services.UploadSegment(segment);

                await nav.PopAsync();
            });
        }

        public void AddPin(decimal latitude, decimal longitude) {
            segment.Locations.Add(new SegmentLocationDto {
                Order = count++,
                Latitude = latitude,
                Longitude = longitude
            });

            ILatLng thisLatLng = new LatLng((double)latitude, (double)longitude);

            if (lastLatLng != null) {
                MapViewModel.AddPolyLine(new[] { lastLatLng, thisLatLng }, Color.Red);
            } else {
                DisplayText = "Now tap to set the next point";
            }

            lastLatLng = thisLatLng;
        }
    }
}
