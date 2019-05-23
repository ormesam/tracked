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
        private string jump;
        private readonly GeoUtility geoUtility;
        private readonly AccelerometerUtility accelerometerUtility;

        public MainPageViewModel()
        {
            mph = 0;
            jump = "";

            geoUtility = new GeoUtility();
            accelerometerUtility = new AccelerometerUtility();

            geoUtility.SpeedChanged += GeoUtility_SpeedChanged;
            accelerometerUtility.JumpDetected += AccelerometerUtility_JumpDetected;
        }

        private void GeoUtility_SpeedChanged(SpeedChangedEventArgs e)
        {
            Mph = (int)Math.Round(e.MetresPerSecond * 2.2369363);
        }

        private void AccelerometerUtility_JumpDetected(JumpEventArgs e)
        {
            Jump += "Jump " + DateTime.Now.ToString() + Environment.NewLine;
            OnPropertyChanged(nameof(Jump));
        }

        public string Jump {
            get { return jump; }
            set {
                if (jump != value)
                {
                    jump = value;
                    OnPropertyChanged(nameof(Jump));
                }
            }
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
            await geoUtility.Start();
            accelerometerUtility.Start();
        }

        public async Task Stop()
        {
            await geoUtility.Stop();
            accelerometerUtility.Stop();

            accelerometerUtility.CheckForEvents();
        }

        public async Task Export()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = accelerometerUtility.GetReadings(),
                Title = "Accelerometer Readings",
            });
        }

        public async Task GoToBluetoothSettings(INavigation nav)
        {
            await nav.PushAsync(new BluetoothSetupScreen());
        }
    }
}