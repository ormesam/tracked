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

                return Math.Round(Ride.Locations.Average(i => i.Mph), 1);
            }
        }

        public double MaxSpeed {
            get {
                if (!Ride.Locations.Any())
                {
                    return 0;
                }

                return Math.Round(Ride.Locations.Max(i => i.Mph), 1);
            }
        }

        public double Distance {
            get {
                if (!Ride.Locations.Any())
                {
                    return 0;
                }

                return Math.Round(Ride.GetLocationSegments().Sum(i => i.Distance), 1);
            }
        }

        public string Time => (Ride.End.Value - Ride.Start.Value).ToString(@"mm\:ss");

        public int JumpCount => Ride.Jumps.Count;

        public string MaxGForce => 0 + "g"; // temp

        public IList<LocationSegmentModel> Locations => Ride.GetLocationSegments();

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
            await Context.UI.GoToMapScreen(nav, Ride);
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
