using System;
using System.Threading.Tasks;
using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Utilities;
using Xamarin.Essentials;

namespace MtbMate
{
    public class MainPageViewModel : ViewModelBase, IDisplay
    {
        private readonly GeoUtility geoUtility;
        private readonly AccelerometerUtility accelerometerUtility;

        public MainPageViewModel()
        {
            geoUtility = new GeoUtility();
            accelerometerUtility = new AccelerometerUtility(this);
        }

        public string Message { get; set; } = "";

        public void ShowJump(AccelerometerReadingModel model)
        {
            Message += Environment.NewLine + "Jump detected!   " + model.ToString();

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