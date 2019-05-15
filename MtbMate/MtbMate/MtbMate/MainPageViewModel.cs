using System;
using System.Threading.Tasks;
using MtbMate.Screens;
using MtbMate.Utilities;
using Xamarin.Essentials;

namespace MtbMate
{
    public class MainPageViewModel : ViewModelBase, IDisplay
    {
        private readonly GeoUtility geoUtility;

        public MainPageViewModel()
        {
            geoUtility = new GeoUtility();
        }

        public async Task Start()
        {
            await geoUtility.Start();
        }

        public async Task Stop()
        {
            await geoUtility.Stop();
        }
    }
}