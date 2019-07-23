using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Gms.Maps.Model;
using MtbMate.Controls;
using MtbMate.Droid.Renderers;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MtbMate.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        private IList<LocationModel> routeCoordinates;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                routeCoordinates = formsMap.RouteCoordinates;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);

            if (!routeCoordinates.Any())
            {
                return;
            }

            var averageSpeed = routeCoordinates.Average(i => i.MetresPerSecond);

            IList<LatLng> latLng = new List<LatLng>();
            var lastColour = GetColour(routeCoordinates.First().MetresPerSecond, averageSpeed);

            foreach (var position in routeCoordinates)
            {
                var thisColour = GetColour(position.MetresPerSecond, averageSpeed);

                if (thisColour != lastColour)
                {
                    PolylineOptions clone = new PolylineOptions();
                    clone.Add(latLng.ToArray());
                    clone.InvokeColor(lastColour);
                    clone.Geodesic(true);

                    NativeMap.AddPolyline(clone);

                    lastColour = thisColour;

                    var last = latLng.LastOrDefault();

                    latLng.Clear();

                    if (last != null)
                    {
                        // need to add the last position to get a complete line
                        latLng.Add(last);
                    }
                }

                latLng.Add(new LatLng(position.Latitude, position.Longitude));
            }

            PolylineOptions options = new PolylineOptions();
            options.Add(latLng.ToArray());
            options.InvokeColor(lastColour);
            options.Geodesic(true);
            NativeMap.AddPolyline(options);
        }

        private Android.Graphics.Color GetColour(double mps, double averageSpeed)
        {
            double greenLimit = averageSpeed;
            double yellowLimit = averageSpeed * 0.6;
            double orangeLimit = averageSpeed * 0.3;

            if (mps > greenLimit)
            {
                return Android.Graphics.Color.Green;
            }

            if (mps > yellowLimit)
            {
                return Android.Graphics.Color.Yellow;
            }

            if (mps > orangeLimit)
            {
                return Android.Graphics.Color.Orange;
            }

            return Android.Graphics.Color.Red;
        }
    }
}