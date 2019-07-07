using System;
using System.Linq;
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

        public override string Title => DisplayName;

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

        public string AverageSpeed => GetAverageSpeed() + "mph";

        public string Distance => GetDistanceInKm() + "km";

        public string Time => (Ride.End.Value - Ride.Start.Value).ToString(@"mm\:ss");

        public int JumpCount => Ride.Jumps.Count;

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

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DisplayName));

                await Context.Model.SaveRide(ride);
            });
        }

        private double GetDistanceInKm()
        {
            if (Ride.Locations.Count < 2)
            {
                return 0;
            }

            double distance = 0;

            for (int i = 1; i < Ride.Locations.Count; i++)
            {
                LocationModel lastLocation = Ride.Locations[i - 1];

                distance += lastLocation.DistanceBetween(Ride.Locations[0]);
            }

            return Math.Round(distance / 1000, 2);
        }

        private double GetAverageSpeed()
        {
            if (Ride.Locations.Count < 2)
            {
                return 0;
            }

            double averageSpeed = Ride.Locations.Sum(i => i.Mph) / Ride.Locations.Count;

            return Math.Round(averageSpeed, 1);
        }
    }
}
