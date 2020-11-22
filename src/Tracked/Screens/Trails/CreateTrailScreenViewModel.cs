using System.Collections.Generic;
using Shared.Dtos;
using Shared.Interfaces;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Tracked.Screens.Trails {
    public class CreateTrailScreenViewModel : ViewModelBase {
        private readonly TrailDto trail;
        private int count;
        private ILatLng lastLatLng;
        private string displayText;

        public RideDto Ride { get; }
        public MapControlViewModel MapViewModel { get; }

        public CreateTrailScreenViewModel(MainContext context, RideDto ride) : base(context) {
            trail = new TrailDto();
            Ride = ride;
            count = 1;
            displayText = "Tap on the map to set a start point";

            MapViewModel = new MapControlViewModel(
                context,
                "Ride",
                ride != null ? PolyUtils.GetMapLocations(Ride.Locations, Ride.Jumps) : new List<MapLocation>(),
                isReadOnly: false,
                showRideFeatures: false,
                mapType: MapType.Satellite,
                canChangeMapType: true);

            MapViewModel.MapTapped += MapViewModel_MapTapped;
        }

        private void MapViewModel_MapTapped(object sender, MapClickedEventArgs e) {
            AddPin(e.Position.Latitude, e.Position.Longitude);
        }

        public string Name {
            get { return trail.Name; }
            set { trail.Name = value; }
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

        public override string Title => "Create Trail";

        public void Save(INavigation nav) {
            if (trail.Locations.Count <= 2) {
                return;
            }

            Context.UI.ShowInputDialog("Trail Name", string.Empty, async (newName) => {
                if (string.IsNullOrWhiteSpace(newName)) {
                    return;
                }

                trail.Name = newName;

                await Context.Services.UploadTrail(trail);

                await nav.PopAsync();
            });
        }

        public void AddPin(double latitude, double longitude) {
            trail.Locations.Add(new TrailLocationDto {
                Order = count++,
                Latitude = latitude,
                Longitude = longitude
            });

            ILatLng thisLatLng = new LatLng(latitude, longitude);

            if (lastLatLng != null) {
                MapViewModel.AddPolyLine(new[] { lastLatLng, thisLatLng }, Color.Red);
            } else {
                DisplayText = "Now tap to set the next point";
            }

            lastLatLng = thisLatLng;
        }
    }
}
