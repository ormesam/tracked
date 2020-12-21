using System;
using System.Threading.Tasks;
using System.Timers;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Tracked.Contexts;
using Tracked.Dependancies;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Screens.Record {
    public class RecordScreenViewModel : ViewModelBase {
        private readonly RideRecorder rideRecorder;
        private readonly bool shouldDetectJumps;
        private readonly Timer timer;

        private RecordStatus status;
        private bool hasAcquiredGpsSignal;

        public RecordScreenViewModel(MainContext context) : base(context) {
            rideRecorder = new RideRecorder(Context);
            shouldDetectJumps = context.Settings.ShouldDetectJumps;
            status = RecordStatus.NotStarted;
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;

            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        }

        private void Current_PositionChanged(object sender, PositionEventArgs e) {
            if (e.Position.Accuracy <= 20) {
                CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;
                HasAcquiredGpsSignal = true;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            OnPropertyChanged(nameof(TimerDisplay));
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

                return (DateTime.UtcNow - rideRecorder.Ride.StartUtc).ToString(@"hh\:mm\:ss");
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
                }
            }
        }

        public void Start() {
            Status = RecordStatus.Running;

            timer.Start();

            rideRecorder.StartRide();
        }

        public async Task Stop() {
            timer.Stop();

            await rideRecorder.StopRide();

            Status = RecordStatus.Complete;

            await StopLocationListening();
        }

        public async Task StartLocationListening() {
            // Has already been started
            if (Status != RecordStatus.NotStarted || CrossGeolocator.Current.IsListening) {
                return;
            }

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, false, new ListenerSettings {
                ActivityType = ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = false,
                ListenForSignificantChanges = false,
                PauseLocationUpdatesAutomatically = false
            });

            DependencyService.Get<INativeForegroundService>().Start();
        }

        public async Task StopLocationListening() {
            if (Status == RecordStatus.Running) {
                return;
            }

            if (CrossGeolocator.Current.IsListening) {
                await CrossGeolocator.Current.StopListeningAsync();
            }

            DependencyService.Get<INativeForegroundService>().Stop();
        }
    }
}
