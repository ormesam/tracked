using System;
using System.Threading.Tasks;
using MtbMate.Models;
using MtbMate.Screens.Bluetooth;
using MtbMate.Screens.Ride;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Contexts
{
    public class UIContext
    {
        private readonly MainContext context;

        public UIContext(MainContext context)
        {
            this.context = context;
        }

        public void ShowInputDialog(string title, string defaultText, Action<string> onOk)
        {
            DependencyService.Get<IPromptUtility>().ShowInputDialog(title, defaultText, onOk);
        }

        public void ShowInputDialog(string title, string defaultText, Func<string, Task<string>> onOk)
        {
            DependencyService.Get<IPromptUtility>().ShowInputDialog(title, defaultText, onOk);
        }

        public async Task GoToRideScreen(INavigation nav)
        {
            await nav.PushAsync(new RideScreen(context));
        }

        public async Task GoToReviewScreen(INavigation nav, RideModel ride)
        {
            await nav.PushAsync(new ReviewScreen(context, ride));
        }

        public async Task GoToBluetoothScreen(INavigation nav)
        {
            await nav.PushAsync(new BluetoothSetupScreen(context));
        }

        public async Task GoToMainPage(INavigation nav)
        {
            await nav.PopToRootAsync();
        }

        public async Task GoToMapScreen(INavigation nav, RideModel ride)
        {
            await nav.PushAsync(new MapScreen(context, ride));
        }
    }
}
