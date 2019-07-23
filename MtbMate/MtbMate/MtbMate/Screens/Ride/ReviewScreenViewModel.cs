using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Essentials;

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

        public string AverageSpeed => Math.Round(Ride.LocationSegments.Average(i => i.Mph), 1) + " mi/h";

        public string MaxSpeed => Math.Round(Ride.LocationSegments.Max(i => i.Mph)) + " mi/h";

        public string Distance => Ride.LocationSegments.Sum(i => i.Distance) + " mi";

        public string Time => (Ride.End.Value - Ride.Start.Value).ToString(@"mm\:ss");

        public int JumpCount => Ride.Jumps.Count;

        public string MaxGForce => 0 + "g"; // temp

        public IList<LocationSegmentModel> Locations => Ride.LocationSegments;

        public async Task Export()
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                File = Ride.GetReadingsFile(),
                Title = Ride.Name ?? "Data Readings",
            });
        }

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
    }
}
