using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.Graphics;
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
        private IList<LocationSegmentModel> routeCoordinates;

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

            var averageSpeed = routeCoordinates.Average(i => i.Mph);
            var maxSpeed = routeCoordinates.Max(i => i.Mph);

            IList<LatLng> latLng = new List<LatLng>();
            var lastColour = Android.Graphics.Color.Black;
            bool firstRun = true;

            foreach (var segment in routeCoordinates)
            {
                var thisColour = GetColour(segment.Mph, averageSpeed);

                if (firstRun || thisColour != lastColour)
                {
                    if (!firstRun)
                    {
                        AddLine(latLng.ToArray(), lastColour);
                    }

                    firstRun = false;

                    lastColour = thisColour;

                    latLng.Clear();

                    latLng.Add(GetLatLon(segment.Start));
                }

                latLng.Add(GetLatLon(segment.End));

                if (segment.Mph == maxSpeed)
                {
                    AddMaxSpeedPin(segment);
                }
            }

            AddLine(latLng.ToArray(), lastColour);
        }

        private void AddMaxSpeedPin(LocationSegmentModel segment)
        {
            MarkerOptions marker = new MarkerOptions();
            marker.SetPosition(GetLatLon(segment.End));
            marker.SetTitle(Math.Round(segment.Mph, 1) + " mi/h");
            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.speed_icon));

            NativeMap.AddMarker(marker);
        }

        private void AddLine(LatLng[] latLng, Android.Graphics.Color olour)
        {
            PolylineOptions options = new PolylineOptions();
            options.Add(latLng.ToArray());
            options.InvokeColor(olour);
            options.Geodesic(true);

            NativeMap.AddPolyline(options);
        }

        private LatLng GetLatLon(LocationModel location)
        {
            return new LatLng(location.Latitude, location.Longitude);
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