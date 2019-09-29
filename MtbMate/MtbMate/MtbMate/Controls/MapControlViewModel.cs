using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Utilities;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MtbMate.Controls {
    public class MapControlViewModel : ViewModelBase {
        private bool registerMapClick;
        private string title;

        public Position InitialLocation { get; }
        public Distance InitialDistance { get; }
        public bool HasScrollEnabled { get; }
        public bool IsShowingUser { get; }
        public bool ShowSpeed { get; }
        public IList<MapLocation> Locations { get; set; }
        public override string Title => title;

        public MapControlViewModel(
            MainContext context,
            string title,
            IList<MapLocation> locations,
            bool isReadonly = true,
            bool showSpeed = true,
            bool showUser = false,
            bool registerMapClick = true)
            : base(context) {

            LatLng centre;

            if (locations.Any()) {
                centre = locations.Midpoint().Point;
                InitialDistance = Distance.FromMiles(0.25);
            } else {
                var lastLocation = CrossGeolocator.Current.GetLastKnownLocationAsync().Result;

                if (lastLocation != null) {
                    centre = new LatLng(lastLocation.Latitude, lastLocation.Longitude);
                    InitialDistance = Distance.FromMiles(0.25);
                } else {
                    centre = new LatLng(57.1499749, -2.1950675);
                    InitialDistance = Distance.FromMiles(20);
                }
            }

            InitialLocation = new Position(centre.Latitude, centre.Longitude);
            HasScrollEnabled = !isReadonly;
            IsShowingUser = showUser;
            ShowSpeed = showSpeed;
            Locations = locations;

            this.title = title;
            this.registerMapClick = registerMapClick;
        }

        public async Task GoToMapScreenAsync(INavigation nav) {
            if (!registerMapClick) {
                return;
            }

            await Context.UI.GoToMapScreenAsync(nav, title, Locations, ShowSpeed);
        }
    }
}
