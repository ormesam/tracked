using MtbMate.Dependancies;
using Xamarin.Forms;

namespace MtbMate.Utilities {
    public static class Toast {
        public static void LongAlert(string message) {
            DependencyService.Get<IToast>().LongAlert(message);
        }

        public static void ShortAlert(string message) {
            DependencyService.Get<IToast>().ShortAlert(message);
        }
    }
}
