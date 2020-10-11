using System;
using System.Globalization;
using Shared.Dtos;
using Xamarin.Forms;

namespace Tracked.Converters {
    public class RouteSvgConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is RideOverviewDto ride) {
                return Format(ride.RouteCanvasWidthSvg, ride.RouteCanvasHeightSvg, ride.RouteSvgPath);
            }

            throw new Exception($"Expected {typeof(RideOverviewDto)} but got {value?.GetType() ?? null}");
        }

        private string Format(int routeCanvasWidthSvg, int routeCanvasHeightSvg, string routeSvgPath) {
            return $"<svg viewBox=\"0 0 {routeCanvasWidthSvg} {routeCanvasHeightSvg}\"><path fill=\"none\" stroke=\"red\" stroke-width=\"4\" d=\"{routeSvgPath}\"/></svg>";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
