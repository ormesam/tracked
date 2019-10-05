using System.Collections.Generic;
using System.Threading.Tasks;
using MtbMate.Accelerometer;
using MtbMate.Contexts;
using MtbMate.Controls;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Screens.Review {
    public class RideScreenViewModel : ViewModelBase {
        private readonly RideRecorder rideController;
        private bool isRunning;
        private bool hasRan;

        public MapControlViewModel MapViewModel { get; }

        private AccelerometerStatus accelerometerStatus;

        public RideScreenViewModel(MainContext context) : base(context) {
            rideController = new RideRecorder(Context.Settings.DetectJumps);
            accelerometerStatus = AccelerometerUtility.Instance.Status;
            isRunning = false;
            hasRan = false;

            MapViewModel = new MapControlViewModel(
                context,
                Title,
                new List<MapLocation>(),
                isReadOnly: false,
                showSpeed: false,
                isShowingUser: true,
                goToMapPageOnClick: false);

            AccelerometerUtility.Instance.StatusChanged += BleAccelerometerUtility_StatusChanged;
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
                    OnPropertyChanged(nameof(ReadyText));
                    OnPropertyChanged(nameof(IsReady));
                    OnPropertyChanged(nameof(CanSeeStartButton));
                }
            }
        }

        public bool IsReady => Context.Settings.DetectJumps ? AccelerometerStatus == AccelerometerStatus.Ready : true;

        public string ReadyText {
            get {
                switch (AccelerometerStatus) {
                    case AccelerometerStatus.NotConnected:
                        return "No accelerometer connected";
                    case AccelerometerStatus.NotReady:
                        return "Connecting to accelerometer...";
                    default:
                        return "Connected";
                }
            }
        }

        public bool IsRunning {
            get { return isRunning; }
            set {
                if (isRunning != value) {
                    isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                    OnPropertyChanged(nameof(CanSeeStartButton));
                }
            }
        }

        public bool HasRan {
            get { return hasRan; }
            set {
                if (hasRan != value) {
                    hasRan = value;
                    OnPropertyChanged(nameof(HasRan));
                    OnPropertyChanged(nameof(CanSeeStartButton));
                }
            }
        }

        public bool CanSeeStartButton => IsReady && !IsRunning && !HasRan;

        public async Task Start() {
            IsRunning = true;
            HasRan = false;
            await rideController.StartRide();
        }

        public async Task Stop(INavigation nav) {
            await rideController.StopRide();

            await nav.PopToRootAsync();
        }
    }
}
