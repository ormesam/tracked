using System;
using System.Globalization;
using Tracked.Models;
using Xamarin.Forms;

namespace Tracked.Converters {
    public class TabImageSourceConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is TabItem item) {
                return (item.IsSelected ? item.ImageName : item.ImageName + "_outline") + ".png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
