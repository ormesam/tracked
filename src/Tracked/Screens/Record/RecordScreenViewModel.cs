using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Tracked.Accelerometer;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;

namespace Tracked.Screens.Record {
    public class RecordScreenViewModel : ViewModelBase {
        private readonly RideRecorder rideController;
        private readonly Stopwatch stopWatch;
        private readonly Timer timer;

        private RecordStatus status;
        private AccelerometerStatus accelerometerStatus;
        private bool hasAcquiredGpsSignal;

        public RecordScreenViewModel(MainContext context) : base(context) {
            rideController = new RideRecorder(Context);
            accelerometerStatus = AccelerometerUtility.Instance.Status;
            status = RecordStatus.NotStarted;
            stopWatch = new Stopwatch();
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;

            AccelerometerUtility.Instance.StatusChanged += BleAccelerometerUtility_StatusChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            OnPropertyChanged(nameof(TimerDisplay));
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e) {
            if (e.Location.AccuracyInMetres < 20) {
                GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;
                HasAcquiredGpsSignal = true;
            }
        }

        private void BleAccelerometerUtility_StatusChanged(AccelerometerStatusChangedEventArgs e) {
            AccelerometerStatus = e.NewStatus;
        }

        public AccelerometerStatus AccelerometerStatus {
            get { return accelerometerStatus; }
            set {
                if (accelerometerStatus != value) {
                    accelerometerStatus = value;
                    OnPropertyChanged(nameof(AccelerometerStatus));
                    OnPropertyChanged(nameof(AccelerometerMessage));
                    OnPropertyChanged(nameof(CanStart));
                }
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

        public bool IsAccelerometerRequired => Context.Settings.DetectJumps;

        public string AccelerometerMessage {
            get {
                switch (AccelerometerStatus) {
                    case AccelerometerStatus.NotConnected:
                        return "No accelerometer connected";
                    case AccelerometerStatus.NotReady:
                        return "Connecting to accelerometer...";
                    default:
                        return "Connected to accelerometer";
                }
            }
        }

        public string TimerDisplay {
            get {
                if (!stopWatch.IsRunning) {
                    return "--:--:--";
                }

                return stopWatch.Elapsed.ToString(@"hh\:mm\:ss");
            }
        }

        public bool CanStart => HasAcquiredGpsSignal &&
            (IsAccelerometerRequired ? AccelerometerStatus == AccelerometerStatus.Ready : true) &&
            Status == RecordStatus.NotStarted;

        public bool CanStop => Status == RecordStatus.Running;

        public bool ShowNotifications => Status == RecordStatus.NotStarted;

        public bool ShowAccelerometerNotification => ShowNotifications && IsAccelerometerRequired;

        public RecordStatus Status {
            get { return status; }
            set {
                if (status != value) {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(CanStart));
                    OnPropertyChanged(nameof(CanStop));
                    OnPropertyChanged(nameof(ShowNotifications));
                    OnPropertyChanged(nameof(ShowAccelerometerNotification));
                }
            }
        }

        public async Task Start() {
            Status = RecordStatus.Running;

            stopWatch.Start();
            timer.Start();

            await rideController.StartRide();
        }

        public async Task Stop() {
            stopWatch.Stop();
            timer.Stop();

            await rideController.StopRide();
            GeoUtility.Instance.Stop();

            Status = RecordStatus.Complete;
        }
    }
}
