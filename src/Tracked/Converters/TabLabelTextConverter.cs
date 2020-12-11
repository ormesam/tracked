using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tracked.Converters {
    public class TabLabelTextConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool boolValue) {
                Application.Current.Resources.TryGetValue("DefaultBackgroundColour", out object colour);
                return boolValue ? colour : Color.Default;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
