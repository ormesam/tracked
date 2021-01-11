using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Tracked.Converters {
    public class RouteSvgConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is string stringValue) {
                return (Geometry)new PathGeometryConverter().ConvertFromInvariantString(stringValue);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
