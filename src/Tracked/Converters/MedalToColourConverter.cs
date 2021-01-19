using System;
using System.Globalization;
using Shared;
using Xamarin.Forms;

namespace Tracked.Converters {
    public class MedalToColourConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Medal medal) {
                object brush;

                switch (medal) {
                    case Medal.Bronze:
                        Application.Current.Resources.TryGetValue("BronzeBrush", out brush);
                        break;
                    case Medal.Silver:
                        Application.Current.Resources.TryGetValue("SilverBrush", out brush);
                        break;
                    case Medal.Gold:
                        Application.Current.Resources.TryGetValue("GoldBrush", out brush);
                        break;
                    default:
                        return null;
                }

                return (SolidColorBrush)brush;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
