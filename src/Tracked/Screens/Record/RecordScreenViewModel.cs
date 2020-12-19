using System;
using System.Threading.Tasks;
using System.Timers;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;

namespace Tracked.Screens.Record {
    public class RecordScreenViewModel : ViewModelBase {
        private readonly RideRecorder rideController;
        private readonly bool shouldDetectJumps;
        private readonly Timer timer;

        private RecordStatus status;
        private bool hasAcquiredGpsSignal;

        public RecordScreenViewModel(MainContext context) : base(context) {
            rideController = new RideRecorder(Context);
            shouldDetectJumps = context.Settings.ShouldDetectJumps;
            status = RecordStatus.NotStarted;
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;

            Context.GeoUtility.LocationChanged += GeoUtility_LocationChanged;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            OnPropertyChanged(nameof(TimerDisplay));
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e) {
            if (e.Location.AccuracyInMetres < 20) {
                Context.GeoUtility.LocationChanged -= GeoUtility_LocationChanged;
                HasAcquiredGpsSignal = true;
            }
        }

        public bool HasAcquiredGpsSignal {
            get { return hasAcquiredGpsSignal; }
            set {
                if (hasAcquiredGpsSignal != value) {
                    hasAcquiredGpsSignal = value;
                    OnPropertyChanged(nameof(HasAcquiredGpsSignal));
                    OnPropertyChanged(nameof(GpsSignalMessage));
                    OnPropertyChanged(nameof(CanStart));
                }
            }
        }

        public string GpsSignalMessage => HasAcquiredGpsSignal ? "GPS Acquired" : "Acquiring GPS";

        public string TimerDisplay {
            get {
                if (Status == RecordStatus.NotStarted) {
                    return "--:--:--";
                }

                return (DateTime.UtcNow - rideController.Ride.StartUtc).ToString(@"hh\:mm\:ss");
            }
        }

        public bool CanStart => HasAcquiredGpsSignal && Status == RecordStatus.NotStarted;

        public bool CanStop => Status == RecordStatus.Running;

        public bool ShowNotifications => Status == RecordStatus.NotStarted;

        public string AccelerometerNotification => shouldDetectJumps ? "Detecting Jumps" : "Not Detecting Jumps";

        public bool ShouldDetectJumps => shouldDetectJumps;

        public RecordStatus Status {
            get { return status; }
            set {
                if (status != value) {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(CanStart));
                    OnPropertyChanged(nameof(CanStop));
                    OnPropertyChanged(nameof(ShowNotifications));
                    OnPropertyChanged(nameof(AccelerometerNotification));
                }
            }
        }

        public void Start() {
            Status = RecordStatus.Running;

            timer.Start();

            rideController.StartRide();
        }

        public async Task Stop() {
            timer.Stop();

            await rideController.StopRide();
            Context.GeoUtility.Stop();

            Status = RecordStatus.Complete;
        }
    }
}
