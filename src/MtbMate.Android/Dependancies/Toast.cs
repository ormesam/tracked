using Android.App;
using Android.Widget;
using Tracked.Droid.Dependancies;
using IToast = Tracked.Dependancies.IToast;

[assembly: Xamarin.Forms.Dependency(typeof(ToastHelper))]
namespace Tracked.Droid.Dependancies {
    public class ToastHelper : IToast {
        public void LongAlert(string message) {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message) {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}