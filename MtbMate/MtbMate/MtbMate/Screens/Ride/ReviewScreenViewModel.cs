using System;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Essentials;

namespace MtbMate.Screens.Ride
{
    public class ReviewScreenViewModel : ViewModelBase
    {
        private RideModel ride;

        public ReviewScreenViewModel(MainContext context, RideModel ride) : base(context)
        {
            this.ride = ride;
        }

        public RideModel Ride {
            get { return ride; }
            set {
                if (ride != value)
                {
                    ride = value;
                    OnPropertyChanged(nameof(Ride));
                }
            }
        }

        public async Task Export()
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                File = ride.GetReadingsFile(),
                Title = "Data Readings",
            });
        }

        public async Task Delete()
        {
            await Context.Model.RemoveRide(Ride);
        }
    }
}
