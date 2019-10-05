using System.Collections.Generic;
using MtbMate.Contexts;
using MtbMate.Controls;
using MtbMate.Models;

namespace MtbMate.Screens.Review {
    public class MapScreenViewModel : MapControlViewModel {
        public MapScreenViewModel(MainContext context, string title, IList<MapLocation> locations, bool showSpeed)
            : base(context,
                title,
                locations,
                isReadOnly: false,
                showSpeed: showSpeed,
                isShowingUser: false,
                goToMapPageOnClick: false,
                canChangeMapType: true) {
        }
    }
}
