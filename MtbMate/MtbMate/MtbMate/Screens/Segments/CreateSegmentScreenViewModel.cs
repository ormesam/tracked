using System;
using System.Collections.ObjectModel;
using System.Linq;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MtbMate.Screens.Segments {
    public class CreateSegmentScreenViewModel : ViewModelBase {
        private readonly SegmentModel segment;
        public readonly RideModel Ride;
        private int count;

        public CreateSegmentScreenViewModel(MainContext context, RideModel ride) : base(context) {
            segment = new SegmentModel();
            Ride = ride;
            Points = new ObservableCollection<Pin>();
            count = 1;
        }

        public string Name {
            get { return segment.Name; }
            set { segment.Name = value; }
        }

        public override string Title => "Create Segment";

        public ObservableCollection<Pin> Points { get; }

        public void Save(INavigation nav) {
            if (!Points.Any()) {
                return;
            }

            segment.Points = Points
                .Select(i => new LatLng(i.Position.Latitude, i.Position.Longitude))
                .ToList();

            Context.UI.ShowInputDialog("Segment Name", string.Empty, async (newName) => {
                if (string.IsNullOrWhiteSpace(newName)) {
                    return;
                }

                segment.Name = newName;
                segment.Created = DateTime.UtcNow;

                await Model.Instance.SaveSegment(segment);

                await nav.PopAsync();
            });
        }

        public void AddPin(double latitude, double longitude) {
            var pin = new Pin {
                Position = new Position(latitude, longitude),
                Label = $"Point {count++}\nTap to delete",
            };

            pin.Clicked += (s, e) => {
                Points.Remove(pin);
            };

            Points.Add(pin);
        }
    }
}
