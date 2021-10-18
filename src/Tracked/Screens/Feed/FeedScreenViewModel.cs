using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Screens.Feed {
    public class FeedScreenViewModel : TabbedViewModelBase {
        private bool isLoading;
        private bool isUploading;
        private ObservableCollection<FeedBaseDto> feed;

        public FeedScreenViewModel(MainContext context) : base(context, TabItemType.Feed) {
            feed = new ObservableCollection<FeedBaseDto>();
        }

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

        public ObservableCollection<FeedBaseDto> Feed {
            get { return feed; }
            set {
                if (feed != value) {
                    feed = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task Load() {
            IsLoading = true;

            try {
                var wrapper = await Context.Services.GetFeed();

                Feed = wrapper.Rides
                    .Cast<FeedBaseDto>()
                    .Concat(wrapper.Follows)
                    .OrderByDescending(i => i.Date)
                    .ToObservable();
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
                    RideFeedDto rideOverview = await Context.Services.SaveRide(upload);
                    await Context.Model.RemoveUploadRide(upload);

                    Feed.Insert(0, rideOverview);

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

        public async Task GoToReview(RideFeedDto ride) {
            await Context.UI.GoToRideReviewScreenAsync(ride.RideId.Value);
        }
    }
}