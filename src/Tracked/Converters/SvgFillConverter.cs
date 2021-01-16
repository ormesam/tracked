using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tracked.Converters {
    public class SvgFillConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Application.Current.Resources.TryGetValue("TextColour", out object textColour);
            Application.Current.Resources.TryGetValue("BackgroundColour", out object defaultColour);

            if (value is bool boolValue) {
                return boolValue ? new SolidColorBrush((Color)textColour) : null;
            }

            return new SolidColorBrush((Color)defaultColour);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
