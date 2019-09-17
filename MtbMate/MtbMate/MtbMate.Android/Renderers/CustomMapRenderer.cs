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
namespace MtbMate.Droid.Renderers {
    public class CustomMapRenderer : MapRenderer {
        private IList<Location> routeCoordinates;
        private bool showSpeed;

        public CustomMapRenderer(Context context) : base(context) {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e) {
            base.OnElementChanged(e);

            if (e.OldElement != null) {
                // Unsubscribe
            }

            if (e.NewElement != null) {
                var formsMap = (CustomMap)e.NewElement;

                routeCoordinates = formsMap.RouteCoordinates
                    .OrderBy(i => i.Timestamp)
                    .ToList();

                showSpeed = formsMap.ShowSpeed;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map) {
            base.OnMapReady(map);

            if (!routeCoordinates.Any()) {
                return;
            }

            var maxSpeed = routeCoordinates.Max(i => i.Mph);

            IList<Android.Gms.Maps.Model.LatLng> latLng = new List<Android.Gms.Maps.Model.LatLng>();
            var lastColour = Android.Graphics.Color.Blue;
            bool firstRun = true;

            foreach (var location in routeCoordinates) {
                var thisColour = showSpeed ? GetMaxSpeedColour(location.Mph, maxSpeed) : Android.Graphics.Color.Blue;

                if (firstRun || thisColour != lastColour) {
                    if (!firstRun) {
                        AddLine(latLng.ToArray(), lastColour);
                    }

                    firstRun = false;

                    lastColour = thisColour;

                    var lastLatLon = latLng.LastOrDefault();

                    latLng.Clear();

                    if (lastLatLon != null) {
                        latLng.Add(lastLatLon);
                    }
                }

                latLng.Add(GetLatLon(location));

                if (showSpeed && location.Mph == maxSpeed) {
                    AddMaxSpeedPin(location);
                }
            }

            AddLine(latLng.ToArray(), lastColour);
        }

        private void AddMaxSpeedPin(Location location) {
            MarkerOptions marker = new MarkerOptions();
            marker.SetPosition(GetLatLon(location));
            marker.SetTitle(Math.Round(location.Mph, 1) + " mi/h");
            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.speed_icon));

            NativeMap.AddMarker(marker);
        }

        private void AddLine(Android.Gms.Maps.Model.LatLng[] latLng, Android.Graphics.Color colour) {
            PolylineOptions options = new PolylineOptions();
            options.Add(latLng.ToArray());
            options.InvokeColor(colour);
            options.Geodesic(true);

            NativeMap.AddPolyline(options);
        }

        private Android.Gms.Maps.Model.LatLng GetLatLon(Location location) {
            return new Android.Gms.Maps.Model.LatLng(location.LatLong.Latitude, location.LatLong.Longitude);
        }

        private Android.Graphics.Color GetMaxSpeedColour(double mph, double maxSpeed) {
            double redLimit = maxSpeed * 0.95;
            double orangeLimit = maxSpeed * 0.85;
            double yellowLimit = maxSpeed * 0.75;

            if (mph > redLimit) {
                return Android.Graphics.Color.Red;
            }

            if (mph > orangeLimit) {
                return Android.Graphics.Color.Orange;
            }

            if (mph > yellowLimit) {
                return Android.Graphics.Color.Yellow;
            }

            return Android.Graphics.Color.Green;
        }
    }
}