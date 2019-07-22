using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace MtbMate.Controls
{
    public class CustomMap : Map
    {
        public IList<Position> RouteCoordinates { get; set; }

        public CustomMap()
        {
            RouteCoordinates = new List<Position>();
        }
    }
}
