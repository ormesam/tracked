using MtbMate.Contexts;
using MtbMate.Models;

namespace MtbMate.Screens.Ride
{
    public class MapScreenViewModel : ViewModelBase
    {
        public RideModel Ride { get; }

        public MapScreenViewModel(MainContext context, RideModel ride) : base(context)
        {
            Ride = ride;
        }

        public override string Title => Ride.DisplayName;
    }
}
