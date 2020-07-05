using System;
using System.Threading.Tasks;
using System.Timers;
using Tracked.Accelerometer;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;

namespace Tracked.Screens.Record {
    public class RecordScreenViewModel : ViewModelBase {
        private readonly RideRecorder rideController;
        private readonly Timer timer;

        private RecordStatus status;
        private AccelerometerStatus accelerometerStatus;
        private bool hasAcquiredGpsSignal;

        public RecordScreenViewModel(MainContext context) : base(context) {
            rideController = new RideRecorder(Context);
            accelerometerStatus = Context.AccelerometerUtility.Status;
            status = RecordStatus.NotStarted;
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;

            Context.AccelerometerUtility.StatusChanged += BleAccelerometerUtility_StatusChanged;
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

        public bool IsAccelerometerRequired => Context.Settings.IsDetectingJumps;

        public string AccelerometerMessage {
            get {
                return AccelerometerStatus switch
                {
                    AccelerometerStatus.BluetoothTurnedOff => "Bluetooth not turned on",
                    AccelerometerStatus.BluetoothTurningOn => "Bluetooth turning on...",
                    AccelerometerStatus.NotConnected => "No accelerometer connected",
                    AccelerometerStatus.Connecting => "Connecting to accelerometer...",
                    AccelerometerStatus.Connected => "Connected to accelerometer",
                    _ => null,
                };
            }
        }

        public string TimerDisplay {
            get {
                if (Status == RecordStatus.NotStarted) {
                    return "--:--:--";
                }

                return (DateTime.UtcNow - rideController.Ride.StartUtc).ToString(@"hh\:mm\:ss");
            }
        }

        public bool CanStart => HasAcquiredGpsSignal &&
            (IsAccelerometerRequired ? AccelerometerStatus == AccelerometerStatus.Connected : true) &&
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

            timer.Start();

            await rideController.StartRide();
        }

        public async Task Stop() {
            timer.Stop();

            await rideController.StopRide();
            Context.GeoUtility.Stop();

            Status = RecordStatus.Complete;
        }
    }
}
