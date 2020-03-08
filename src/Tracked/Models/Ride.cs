using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Tracked.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Ride {
        [JsonProperty]
        public int? RideId { get; set; }
        [JsonProperty]
        public Guid? Id { get; set; }
        [JsonProperty]
        public DateTime Start { get; set; }
        [JsonProperty]
        public DateTime End { get; set; }
        [JsonProperty]
        public IList<Location> Locations { get; set; }
        [JsonProperty]
        public IList<Jump> Jumps { get; set; }
        [JsonProperty]
        public IList<AccelerometerReading> AccelerometerReadings { get; set; }

        public string DisplayName => Start.ToString("dd/MM/yy HH:mm");

        //public bool ShowAttempts => Model.Instance.SegmentAttempts
        //    .Where(i => i.RideId == Id)
        //    .Any();

        public IList<Location> MovingLocations => Locations
            .Where(i => i.Mph >= 1)
            .ToList();

        //public IList<Medal> Medals => Model.Instance.SegmentAttempts
        //    .Where(i => i.RideId == Id)
        //    .Select(i => i.Medal)
        //    .ToList();

        // At the moment we calculate these on demand as they are changing all the time,
        // however it probably makes sense to store them eventually
        ////public IList<IAchievement> Achievements => Model.Instance.Achievements
        ////    .Where(i => i.Check(this))
        ////    .ToList();

        public Ride() {
            Locations = new List<Location>();
            Jumps = new List<Jump>();
            AccelerometerReadings = new List<AccelerometerReading>();
        }
    }
}
