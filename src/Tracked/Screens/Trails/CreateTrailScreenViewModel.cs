using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Dtos;
using Shared.Interfaces;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Tracked.Screens.Trails {
    public class CreateTrailScreenViewModel : MapViewModelBase {
        private readonly TrailDto trail;
        private int count;
        private ILatLng lastLatLng;
        private string displayText;

        public RideDto Ride { get; }

        public CreateTrailScreenViewModel(MainContext context, RideDto ride) : base(context) {
            trail = new TrailDto();
            Ride = ride;
            count = 1;
            displayText = "Tap on the map to set a start point";

            MapTapped += MapViewModel_MapTapped;
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

        public async Task Save(INavigation nav) {
            if (trail.Locations.Count <= 2) {
                return;
            }

            string newName = await Context.UI.ShowPromptAsync("Trail Name", null, string.Empty);

            if (string.IsNullOrWhiteSpace(newName)) {
                return;
            }

            trail.Name = newName;

            try {
                await Context.Services.UploadTrail(trail);

                await nav.PopAsync();
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            }
        }

        public void AddPin(double latitude, double longitude) {
            trail.Locations.Add(new TrailLocationDto {
                Order = count++,
                Latitude = latitude,
                Longitude = longitude
            });

            ILatLng thisLatLng = new LatLng(latitude, longitude);

            if (lastLatLng != null) {
                CreatePolyline(new MapPolyline {
                    Positions = new List<ILatLng>() { lastLatLng, thisLatLng },
                    Colour = Color.Red,
                    Width = 10,
                });
            } else {
                DisplayText = "Now tap to set the next point";
            }

            lastLatLng = thisLatLng;
        }

        protected override ILatLng Centre => Ride == null ? base.Centre : Ride.Locations.Midpoint();

        protected override IEnumerable<MapPin> GetPins() {
            return new List<MapPin>();
        }

        protected override IEnumerable<MapPolyline> GetPolylines() {
            if (Ride != null) {
                yield return new MapPolyline {
                    Colour = Color.Blue,
                    Positions = Ride.Locations,
                    Width = 10,
                };
            }
        }
    }
}
