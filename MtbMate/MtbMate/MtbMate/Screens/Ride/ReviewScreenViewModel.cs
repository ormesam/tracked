using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MtbMate.Screens.Ride
{
    public class ReviewScreenViewModel : ViewModelBase
    {
        public readonly RideModel Ride;

        public ReviewScreenViewModel(MainContext context, RideModel ride) : base(context)
        {
            Ride = ride;
        }

        public override string Title => DisplayName;

        public string DisplayName => Ride.DisplayName;

        public double AverageSpeed {
            get {
                if (!Ride.Locations.Any())
                {
                    return 0;
                }

                return Ride.Locations.Average(i => i.Mph);
            }
        }

        public double MaxSpeed {
            get {
                if (!Ride.Locations.Any())
                {
                    return 0;
                }

                return Ride.Locations.Max(i => i.Mph);
            }
        }

        public double Distance {
            get {
                if (!Ride.Locations.Any())
                {
                    return 0;
                }

                return Ride.GetLocationSteps().Sum(i => i.Distance);
            }
        }

        public string Time => (Ride.End.Value - Ride.Start.Value).ToString(@"mm\:ss");

        public int JumpCount => Ride.Jumps.Count;

        public string MaxGForce => 0 + "g"; // temp

        public async Task Delete()
        {
            await Context.Model.RemoveRide(Ride);
        }

        public void ChangeName()
        {
            Context.UI.ShowInputDialog("Change Name", Ride.Name, async (newName) =>
            {
                Ride.Name = newName;

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DisplayName));

                await Context.Model.SaveRide(Ride);
            });
        }

        public async Task GoToMapScreen(INavigation nav)
        {
            await Context.UI.GoToMapScreenAsync(nav, Ride);
        }

        public async Task Export()
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                File = Ride.GetReadingsFile(),
                Title = Ride.Name ?? "Data Readings",
            });
        }

        public async Task ExportLocation()
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                File = Ride.GetLocationFile(),
                Title = Ride.Name ?? "Data Readings",
            });
        }
    }
}
