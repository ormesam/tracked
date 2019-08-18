using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;

namespace MtbMate.Screens.Segments
{
    public class CreateSegmentScreenViewModel : ViewModelBase
    {
        private SegmentModel segment;
        private RideModel selectedRide;

        public CreateSegmentScreenViewModel(MainContext context) : base(context)
        {
            segment = new SegmentModel
            {
                Created = DateTime.UtcNow,
            };
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
                if (selectedRide != value)
                {
                    selectedRide = value;
                    OnPropertyChanged(nameof(SelectedRide));
                    OnPropertyChanged(nameof(ShowRideList));
                }
            }
        }

        public bool ShowRideList => SelectedRide == null;

        public async Task Save()
        {
            await Context.Model.SaveSegments(segment);
        }
    }
}
