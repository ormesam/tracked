using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Screens.Rides {
    public class RideOverviewScreenViewModel : TabbedViewModelBase {
        private bool isLoading;
        private bool isUploading;

        public RideOverviewScreenViewModel(MainContext context) : base(context) {
            Rides = new ObservableCollection<RideOverviewDto>();
        }

        protected override TabItemType SelectedTab => TabItemType.Rides;

        public bool IsLoading {
            get { return isLoading; }
            set {
                if (value != isLoading) {
                    isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public bool IsUploading {
            get { return isUploading; }
            set {
                if (isUploading != value) {
                    isUploading = value;
                    OnPropertyChanged(nameof(IsUploading));
                    OnPropertyChanged(nameof(UploadText));
                    OnPropertyChanged(nameof(ShowUploadCount));
                    OnPropertyChanged(nameof(PendingUploudCount));
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public string UploadText {
            get { return $"Uploading {PendingUploudCount} ride{(PendingUploudCount > 1 ? "s" : "")}..."; }
        }

        public int PendingUploudCount {
            get { return Context.Model.PendingRideUploads.Count; }
        }

        public bool ShowUploadCount => PendingUploudCount > 0;

        public ICommand RefreshCommand {
            get { return new Command(async () => await Load()); }
        }

        public ObservableCollection<RideOverviewDto> Rides { get; set; }

        public async Task Load() {
            IsLoading = true;

            try {
                Rides.Clear();
                var rides = await Context.Services.GetRideOverviews();

                foreach (var ride in rides) {
                    Rides.Add(ride);
                }
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            }

            IsLoading = false;

            await UploadRides();
        }

        private async Task UploadRides() {
            var uploads = Context.Model.PendingRideUploads
                .OrderBy(i => i.StartUtc)
                .ToList();

            IsUploading = true;

            foreach (var upload in uploads) {
                try {
                    RideOverviewDto rideOverview = await Context.Services.UploadRide(upload);
                    await Context.Model.RemoveUploadRide(upload);

                    Rides.Insert(0, rideOverview);

                    OnPropertyChanged(nameof(PendingUploudCount));
                    OnPropertyChanged(nameof(UploadText));
                } catch (ServiceException ex) {
                    Toast.LongAlert(ex.Message);
                }
            }

            IsUploading = false;
        }

        public async Task GoToCreateRide() {
            await Context.UI.GoToRecordScreenAsync();
        }

        public async Task GoToReview(RideOverviewDto ride) {
            await Context.UI.GoToRideReviewScreenAsync(ride.RideId.Value);
        }
    }
}