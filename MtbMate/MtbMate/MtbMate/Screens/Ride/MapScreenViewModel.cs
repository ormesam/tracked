using MtbMate.Contexts;
using MtbMate.Models;
using System.Collections.Generic;
using System.Linq;

namespace MtbMate.Screens.Ride
{
    public class MapScreenViewModel : ViewModelBase
    {
        private readonly string title;

        public bool ShowSpeed { get; }
        public IList<LocationModel> Locations { get; }

        public MapScreenViewModel(
            MainContext context,
            RideModel ride) : base(context) {

            title = ride.Name;
            ShowSpeed = true;
            Locations = ride.Locations;
        }

        public MapScreenViewModel(
            MainContext context,
            string title,
            IList<LatLongModel> locations) : base(context) {

            this.title = title;
            ShowSpeed = false;

            Locations = locations
                .Select(i => new LocationModel {
                    LatLong = i,
                })
                .ToList();
        }

        public override string Title => title;
    }
}
