using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

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

        public ObservableCollection<Pin> Locations {
            get {
                return Ride.Locations
                    .Select(i => new Pin
                    {
                        Position = new Position(i.Latitude, i.Longitude),
                        Label = i.Mph + "mph",
                    })
                    .ToObservable();
            }
        }

        public async Task Export()
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                File = ride.GetReadingsFile(),
                Title = ride.Name ?? "Data Readings",
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

                distance += lastLocation.DistanceBetween(Ride.Locations[i]);
            }

            return Math.Round(distance, 2);
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
