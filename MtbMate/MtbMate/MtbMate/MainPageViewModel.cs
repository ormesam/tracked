using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Utilities;
using System;
using System.Threading.Tasks;

namespace MtbMate
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
            Jump = DateTime.Now.ToString() + "   Jumped!";
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
        }
    }
}