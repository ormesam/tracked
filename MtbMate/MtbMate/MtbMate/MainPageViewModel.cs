using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Utilities;
using System;
using System.Threading.Tasks;

namespace MtbMate
{
    public class MainPageViewModel : ViewModelBase, IDisplay
    {
        private int mph;
        private readonly GeoUtility geoUtility;
        private readonly AccelerometerUtility accelerometerUtility;

        public MainPageViewModel()
        {
            mph = 0;

            geoUtility = new GeoUtility();
            accelerometerUtility = new AccelerometerUtility(this);

            geoUtility.SpeedChanged += GeoUtility_SpeedChanged;
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

        public string Message { get; set; } = "";

        public void ShowJump(AccelerometerReadingModel model)
        {
            // Message += Environment.NewLine + "Jump detected!   " + model.ToString();

            OnPropertyChanged(nameof(Message));
        }

        public void ShowMph(double mph)
        {
            Message = Math.Round(mph) + "mph";

            OnPropertyChanged(nameof(Message));
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
        }
    }
}