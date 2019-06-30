using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Screens.Bluetooth;
using MtbMate.Utilities;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MtbMate.Home
{
    public class MainPageViewModel : ViewModelBase
    {
        private int mph;
        private RideModel ride;

        public MainPageViewModel(MainContext context) : base(context)
        {
            mph = 0;

            GeoUtility.Instance.SpeedChanged += GeoUtility_SpeedChanged;
        }

        private void GeoUtility_SpeedChanged(SpeedChangedEventArgs e)
        {
            Mph = (int)Math.Round(e.MetresPerSecond * 2.2369363);
        }

        public int Mph {
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
            ride = new RideModel();

            Context.Rides.Add(ride);

            await ride.StartRide();
        }

        public async Task Stop()
        {
            await ride?.StopRide();
        }

        public async Task Export()
        {
            if (ride == null)
            {
                return;
            }

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = ride.GetReadings(),
                Title = "Accelerometer Readings",
            });
        }

        public async Task GoToBluetoothSettings(INavigation nav)
        {
            await nav.PushAsync(new BluetoothSetupScreen(Context));
        }
    }
}