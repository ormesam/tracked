using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AppCenter.Crashes;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Screens;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Home {
    public class MainPageViewModel : ViewModelBase {
        private bool isRefreshing;
        private bool isUploading;

        public MainPageViewModel(MainContext context) : base(context) {
            Rides = new ObservableCollection<RideOverviewDto>();
            Rides.CollectionChanged += Rides_CollectionChanged;
        }

        private void Rides_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged(nameof(HasRides));
        }

        public bool IsRefreshing {
            get { return isRefreshing; }
            set {
                if (value != isRefreshing) {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
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
                }
            }
        }

        public string UploadText {
            get { return $"Uploading {PendingUploudCount} ride{(PendingUploudCount > 1 ? "s" : "")}..."; }
        }

        public int PendingUploudCount {
            get { return Model.Instance.PendingRideUploads.Count; }
        }

        public bool ShowUploadCount => PendingUploudCount > 0;

        public ICommand RefreshCommand {
            get { return new Command(async () => await Load()); }
        }

        public ObservableCollection<RideOverviewDto> Rides { get; set; }

        public bool HasRides => Rides.Any();

        public async Task Load() {
            IsRefreshing = true;

            try {
                Rides.Clear();
                var rides = await Context.Services.GetRideOverviews();

                foreach (var ride in rides) {
                    Rides.Add(ride);
                }
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            }

            IsRefreshing = false;

            await UploadRides();
        }

        private async Task UploadRides() {
            var uploads = Model.Instance.PendingRideUploads
                .OrderBy(i => i.StartUtc)
                .ToList();

            IsUploading = true;

            foreach (var upload in uploads) {
                try {
                    RideOverviewDto rideOverview = await Context.Services.UploadRide(upload);
                    await Model.Instance.RemoveUploadRide(upload);

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