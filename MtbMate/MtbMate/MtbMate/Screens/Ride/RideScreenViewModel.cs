using System;
using System.Threading.Tasks;
using MtbMate.Accelerometer;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Essentials;

namespace MtbMate.Screens.Ride
{
    public class RideScreenViewModel : ViewModelBase
    {
        private readonly RideModel ride;
        private bool isRunning;
        private bool hasRan;
        private double mph;
        private AccelerometerStatus accelerometerStatus;

        public RideScreenViewModel(MainContext context) : base(context)
        {
            ride = new RideModel();
            accelerometerStatus = BleAccelerometerUtility.Instance.Status;
            isRunning = false;
            hasRan = false;
            mph = 0;

            GeoUtility.Instance.SpeedChanged += GeoUtility_SpeedChanged;
            BleAccelerometerUtility.Instance.StatusChanged += BleAccelerometerUtility_StatusChanged;
        }

        private void BleAccelerometerUtility_StatusChanged(AccelerometerStatusChangedEventArgs e)
        {
            AccelerometerStatus = e.NewStatus;
        }

        private void GeoUtility_SpeedChanged(SpeedChangedEventArgs e)
        {
            Mph = Math.Round(e.MetresPerSecond * 2.237, 1);
        }

        public AccelerometerStatus AccelerometerStatus {
            get { return accelerometerStatus; }
            set {
                if (accelerometerStatus != value)
                {
                    accelerometerStatus = value;
                    OnPropertyChanged(nameof(AccelerometerStatus));
                    OnPropertyChanged(nameof(ReadyText));
                    OnPropertyChanged(nameof(IsReady));
                    OnPropertyChanged(nameof(CanSeeStartButton));
                }
            }
        }

        public bool IsReady => AccelerometerStatus == AccelerometerStatus.Ready;

        public string ReadyText {
            get {
                switch (AccelerometerStatus)
                {
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
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                    OnPropertyChanged(nameof(CanSeeStartButton));
                }
            }
        }

        public bool HasRan {
            get { return hasRan; }
            set {
                if (hasRan != value)
                {
                    hasRan = value;
                    OnPropertyChanged(nameof(HasRan));
                    OnPropertyChanged(nameof(CanSeeStartButton));
                }
            }
        }

        public double Mph {
            get { return mph; }
            set {
                if (mph != value)
                {
                    mph = value;
                    OnPropertyChanged(nameof(Mph));
                }
            }
        }

        public bool CanSeeStartButton => IsReady && !IsRunning && !HasRan;

        public async Task Start()
        {
            IsRunning = true;
            HasRan = false;
            await ride.StartRide();
        }

        public async Task Stop()
        {
            await ride.StopRide();
            HasRan = true;
            IsRunning = false;
        }

        public async Task Save()
        {
            await Context.Model.SaveRide(ride);
        }

        public async Task Export()
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                File = ride.GetReadingsFile(),
                Title = "Data Readings",
            });
        }
    }
}
