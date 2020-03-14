using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Screens;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Home {
    public class MainPageViewModel : ViewModelBase {
        private bool isRefreshing;

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

        public ICommand RefreshCommand {
            get { return new Command(async () => await Load()); }
        }

        public ObservableCollection<RideOverviewDto> Rides { get; set; }

        public bool HasRides => Rides.Any();

        public async Task Load() {
            IsRefreshing = true;

            try {
                // upload rides
                Rides.Clear();

                foreach (var upload in Model.Instance.PendingRideUploads) {
                    Rides.Add(new RideOverviewDto {
                        ClientId = upload.Id,
                        IsAwaitingUpload = true,
                        StartUtc = upload.StartUtc,
                    });
                }

                var rides = await Context.Services.GetRideOverviews();

                foreach (var ride in rides) {
                    Rides.Add(ride);
                }

                IsRefreshing = false;
            } catch (ServiceException ex) {
                IsRefreshing = false;
                Toast.LongAlert(ex.Message);
            }

            var uploads = Model.Instance.PendingRideUploads
                .OrderBy(i => i.StartUtc)
                .ToList();

            foreach (var upload in uploads) {
                try {
                    var uploadOverview = Rides.SingleOrDefault(i => i.ClientId == upload.Id);

                    if (uploadOverview == null) {
                        continue;
                    }

                    uploadOverview.IsUploading = true;

                    RideOverviewDto rideOverview = await Context.Services.UploadRide(upload);
                    await Model.Instance.RemoveUploadRide(upload);

                    Rides.Remove(uploadOverview);
                    Rides.Insert(0, rideOverview);
                } catch (ServiceException ex) {
                    Toast.LongAlert(ex.Message);
                }
            }
        }

        public async Task GoToCreateRide() {
            await Context.UI.GoToRecordScreenAsync();
        }

        public async Task GoToReview(RideOverviewDto ride) {
            if (ride.IsAwaitingUpload) {
                return;
            }

            await Context.UI.GoToRideReviewScreenAsync(ride.RideId.Value);
        }
    }
}