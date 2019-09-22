namespace MtbMate.Models {
    public class SegmentLocation {
        public int Order { get; set; }
        public LatLng Point { get; set; }

        public SegmentLocation(int order, double lat, double lng) {
            Order = order;
            Point = new LatLng(lat, lng);
        }
    }
}
