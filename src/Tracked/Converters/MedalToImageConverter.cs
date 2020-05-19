using System;
using System.Globalization;
using Shared;
using Xamarin.Forms;

namespace Tracked.Converters {
    public class MedalToImageConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Medal medal) {
                switch (medal) {
                    case Medal.Bronze:
                        return "bronze.png";
                    case Medal.Silver:
                        return "silver.png";
                    case Medal.Gold:
                        return "gold.png";
                    default:
                        return null;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
