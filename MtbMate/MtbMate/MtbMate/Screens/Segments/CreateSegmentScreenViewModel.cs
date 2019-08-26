using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MtbMate.Screens.Segments
{
    public class CreateSegmentScreenViewModel : ViewModelBase
    {
        private SegmentModel segment;
        private RideModel selectedRide;
        private int lowerIndex;
        private int upperIndex;

        public CreateSegmentScreenViewModel(MainContext context) : base(context) {
            segment = new SegmentModel();
        }

        public ObservableCollection<RideModel> Rides => Context.Model.Rides
            .OrderByDescending(i => i.Start)
            .ToObservable();

        public string Name {
            get { return segment.Name; }
            set { segment.Name = value; }
        }

        public override string Title => "Create Segment";

        public RideModel SelectedRide {
            get { return selectedRide; }
            set {
                if (selectedRide != value) {
                    selectedRide = value;

                    LowerIndex = 0;
                    UpperIndex = Locations?.Count - 1 ?? 0;

                    OnPropertyChanged(nameof(SelectedRide));
                    OnPropertyChanged(nameof(ShowRideList));
                    OnPropertyChanged(nameof(FilteredLocations));
                    OnPropertyChanged(nameof(PointCount));
                }
            }
        }

        private IList<LocationModel> Locations {
            get {
                if (SelectedRide == null) {
                    return null;
                }

                return SelectedRide.Locations
                    .Where(i => i.Mph >= 1)
                    .ToList();
            }
        }

        public IList<Pin> FilteredLocations {
            get {
                if (Locations == null) {
                    return new List<Pin>();
                }

                return Locations
                    .Select(i => new Pin {
                        Position = new Position(i.LatLong.Latitude, i.LatLong.Longitude),
                        Label = i.Timestamp.ToShortTimeString(),
                    })
                    .ToList()
                    .GetRange(lowerIndex, (upperIndex - lowerIndex) + 1);
            }
        }

        public int PointCount => Locations?.Count - 1 ?? 0;

        public bool ShowRideList => SelectedRide == null;

        public int LowerIndex {
            get { return lowerIndex; }
            set {
                if (lowerIndex != value) {
                    lowerIndex = value;
                    OnPropertyChanged(nameof(LowerIndex));
                    OnPropertyChanged(nameof(FilteredLocations));
                }
            }
        }

        public int UpperIndex {
            get { return upperIndex; }
            set {
                if (upperIndex != value) {
                    upperIndex = value;
                    OnPropertyChanged(nameof(UpperIndex));
                    OnPropertyChanged(nameof(FilteredLocations));
                }
            }
        }

        public void Save(INavigation nav) {
            segment.Points = SelectedRide.Locations
                .Where(i => i.Mph >= 1)
                .Select(i => i.LatLong)
                .ToList()
                .GetRange(lowerIndex, (upperIndex - lowerIndex) + 1);

            if (!segment.Points.Any()) {
                return;
            }

            Context.UI.ShowInputDialog("Segment Name", string.Empty, async (newName) => {
                if (string.IsNullOrWhiteSpace(newName)) {
                    return;
                }

                segment.Name = newName;
                segment.Created = DateTime.UtcNow;

                await Context.Model.SaveSegment(segment);

                await nav.PopAsync();
            });
        }
    }
}
