using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tracked.Converters {
    public class BoolToGpsColourConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool boolValue) {
                return boolValue ? Color.FromHex("#00b300") : Color.FromHex("#ffa500");
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
