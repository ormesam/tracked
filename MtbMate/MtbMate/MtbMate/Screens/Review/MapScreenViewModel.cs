using System.Collections.Generic;
using MtbMate.Contexts;
using MtbMate.Controls;
using MtbMate.Models;

namespace MtbMate.Screens.Review {
    public class MapScreenViewModel : ViewModelBase {
        public MapControlViewModel MapViewModel { get; }

        public MapScreenViewModel(MainContext context, string title, IList<MapLocation> locations, bool showSpeed)
            : base(context) {

            MapViewModel = new MapControlViewModel(
                context,
                title,
                locations,
                isReadonly: false,
                showSpeed: showSpeed,
                showUser: false,
                registerMapClick: false);
        }
    }
}
