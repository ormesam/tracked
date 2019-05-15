using System;
using MtbMate.Screens;
using MtbMate.Utilities;

namespace MtbMate
{
    public class MainPageViewModel : ViewModelBase, IDisplay
    {
        private decimal speed;
        private readonly GeoUtility geoUtility;

        public MainPageViewModel()
        {
            geoUtility = new GeoUtility(this);
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

        public void UpdateSpeed(decimal mph)
        {
            Speed = speed;
        }

        public void Start()
        {
            geoUtility.Start();
        }

        public void Stop()
        {
            geoUtility.Stop();
        }
    }
}