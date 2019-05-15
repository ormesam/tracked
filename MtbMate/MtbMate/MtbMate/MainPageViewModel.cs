using System;
using System.Threading.Tasks;
using MtbMate.Screens;
using MtbMate.Utilities;
using Xamarin.Essentials;

namespace MtbMate
{
    public class MainPageViewModel : ViewModelBase, IDisplay
    {
        private decimal speed;
        private string error;
        private decimal mph;
        private readonly GeoUtility geoUtility;

        public MainPageViewModel()
        {
            geoUtility = new GeoUtility(this);
            speed = 100;
            error = "";
        }

        public decimal Speed {
            get { return speed; }
            set {
                if (speed != value)
                {
                    speed = value;
                    OnPropertyChanged(nameof(Speed));
                }
            }
        }

        public decimal Mph {
            get { return mph; }
            set {
                if (mph != value)
                {
                    mph = value;
                    OnPropertyChanged(nameof(Mph));
                }
            }
        }

        public string Error {
            get { return error; }
            set {
                if (error != value)
                {
                    error = value;
                    OnPropertyChanged(nameof(Error));
                }
            }
        }

        public void UpdateSpeed(decimal mph)
        {
            Speed = mph;
            Mph = mph * 2.237m;
        }

        public async Task Start()
        {
            await geoUtility.Start();
        }

        public async Task Stop()
        {
            await geoUtility.Stop();
        }

        public void ShowError(string error)
        {
            Error = error;
        }
    }
}