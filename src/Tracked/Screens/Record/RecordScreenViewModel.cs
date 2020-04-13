using System.Threading.Tasks;
using Tracked.Accelerometer;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;

namespace Tracked.Screens.Record {
    public class RecordScreenViewModel : ViewModelBase {
        private readonly RideRecorder rideController;

        private RecordStatus status;
        private AccelerometerStatus accelerometerStatus;
        private bool hasAcquiredGpsSignal;

        public RecordScreenViewModel(MainContext context) : base(context) {
            rideController = new RideRecorder(Context);
            accelerometerStatus = AccelerometerUtility.Instance.Status;
            status = RecordStatus.NotStarted;

            AccelerometerUtility.Instance.StatusChanged += BleAccelerometerUtility_StatusChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;
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
                    OnPropertyChanged(nameof(IsReady));
                    OnPropertyChanged(nameof(CanSeeStartButton));
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

        public bool IsReady => HasAcquiredGpsSignal && IsAccelerometerRequired ? AccelerometerStatus == AccelerometerStatus.Ready : true;

        public RecordStatus Status {
            get { return status; }
            set {
                if (status != value) {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(CanSeeStartButton));
                }
            }
        }

        public bool CanSeeStartButton => IsReady && Status == RecordStatus.NotStarted;

        public async Task Start() {
            Status = RecordStatus.Running;

            await rideController.StartRide();
        }

        public async Task Stop() {
            await rideController.StopRide();
            GeoUtility.Instance.Stop();

            Status = RecordStatus.Complete;
        }
    }
}
