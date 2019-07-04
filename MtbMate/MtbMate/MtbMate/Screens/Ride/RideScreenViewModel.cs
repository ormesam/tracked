using System;
using System.Threading.Tasks;
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

        public RideScreenViewModel(MainContext context) : base(context)
        {
            ride = new RideModel();
            isRunning = false;
            hasRan = false;
            mph = 0;

            GeoUtility.Instance.SpeedChanged += GeoUtility_SpeedChanged;
        }

        private void GeoUtility_SpeedChanged(SpeedChangedEventArgs e)
        {
            Mph = Math.Round(e.MetresPerSecond * 2.237, 1);
        }

        public bool IsRunning {
            get { return isRunning; }
            set {
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
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
            await Context.Model.AddRide(ride);
        }

        public async Task Export()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = ride.GetReadings(),
                Title = "Accelerometer Readings",
            });
        }
    }
}
