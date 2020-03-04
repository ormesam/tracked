using Android.App;
using Android.Widget;
using MtbMate.Droid.Dependancies;
using IToast = MtbMate.Dependancies.IToast;

[assembly: Xamarin.Forms.Dependency(typeof(ToastHelper))]
namespace MtbMate.Droid.Dependancies {
    public class ToastHelper : IToast {
        public void LongAlert(string message) {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message) {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}