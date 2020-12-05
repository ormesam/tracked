using System;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Plugin.CurrentActivity;
using Tracked.Dependancies;
using Tracked.Droid.Dependancies;

[assembly: Xamarin.Forms.Dependency(typeof(PromptUtility))]
namespace Tracked.Droid.Dependancies {
    public class PromptUtility : IPromptUtility {
        public void ShowInputDialog(string title, string defaultValue, Action<string> onOk) {
            EditText et = new EditText(CrossCurrentActivity.Current.Activity);
            et.Text = defaultValue;

            AlertDialog.Builder ad = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
            ad.SetTitle(title);
            ad.SetView(et);

            ad.SetPositiveButton("Ok", (senderAlert, args) => {
                onOk(et.Text);
            });

            ad.SetNegativeButton("Cancel", (senderAlert, args) => {
                ad.Dispose();
            });

            ad.Show();
        }

        public void ShowInputDialog(string title, string defaultValue, Func<string, Task<string>> onOk) {
            EditText et = new EditText(CrossCurrentActivity.Current.Activity);
            et.Text = defaultValue;

            AlertDialog.Builder ad = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
            ad.SetTitle(title);
            ad.SetView(et);

            ad.SetPositiveButton("Ok", async (senderAlert, args) => {
                await onOk(et.Text);
            });

            ad.SetNegativeButton("Cancel", (senderAlert, args) => {
                ad.Dispose();
            });

            ad.Show();
        }
    }
}