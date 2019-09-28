using System.Collections.Generic;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MtbMate.Controls {
    public class MapControlViewModel : ViewModelBase {
        private bool registerMapClick;
        private string title;

        public Position InitialLocation { get; }
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

            LatLng centre = locations.Midpoint().Point;

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
