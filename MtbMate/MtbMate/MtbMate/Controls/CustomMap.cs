﻿using System.Collections.Generic;
using MtbMate.Models;
using Xamarin.Forms.Maps;

namespace MtbMate.Controls
{
    public class CustomMap : Map
    {
        public IList<LocationSegmentModel> RouteCoordinates { get; set; }

        public CustomMap()
        {
            RouteCoordinates = new List<LocationSegmentModel>();
            MapType = MapType.Street;
        }
    }
}