using System;

namespace MtbMate.Models
{
    public class SegmentAttemptModel
    {
        public Guid? Id { get; set; }
        public Guid? SegmentId { get; set; }
        public Guid? RideId { get; set; }
        public DateTime Created { get; set; }
        public string DisplayName => Created.ToString("dd/MM/yy HH:mm");
    }
}
