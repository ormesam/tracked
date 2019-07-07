using System;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        public string DisplayName => Ride.DisplayName;

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

        public void ChangeName()
        {
            Context.UI.ShowInputDialog("Change Name", ride.Name, async (newName) =>
            {
                ride.Name = newName;

                OnPropertyChanged(nameof(DisplayName));

                await Context.Model.SaveRide(ride);
            });
        }
    }
}
