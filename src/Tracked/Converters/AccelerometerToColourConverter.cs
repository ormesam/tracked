using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tracked.Converters {
    public class AccelerometerToColourConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool boolValue) {
                Application.Current.Resources.TryGetValue("Green", out object green);
                Application.Current.Resources.TryGetValue("Orange", out object orange);

                return boolValue ? green : orange;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
