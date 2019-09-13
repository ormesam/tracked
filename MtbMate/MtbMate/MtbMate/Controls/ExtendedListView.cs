using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MtbMate.Controls {
    public class ExtendedListView : ListView {
        public static readonly BindableProperty IsScrollEnabledProperty = BindableProperty.Create(
            nameof(IsScrollEnabledProperty),
            typeof(bool),
            typeof(TextViewCell),
            true);

        public bool IsScrollEnabled {
            get { return (bool)GetValue(IsScrollEnabledProperty); }
            set { SetValue(IsScrollEnabledProperty, value); }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(ItemsSource)) {
                SetHeight();
            }
        }

        private void SetHeight() {
            if (ItemsSource == null) {
                HeightRequest = 0;

                return;
            }

            HeightRequest = (40 * ItemsSource.Cast<object>().Count()) + 10;
        }
    }
}
