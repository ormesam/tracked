using System.Collections.Generic;

namespace Shared.Dtos {
    public class TrailDto {
        public int? TrailId { get; set; }
        public string Name { get; set; }
        public IList<TrailLocationDto> Locations { get; set; }
        public IList<TrailAttemptDto> Attempts { get; set; }

        public TrailDto() {
            Locations = new List<TrailLocationDto>();
            Attempts = new List<TrailAttemptDto>();
        }
    }
}
