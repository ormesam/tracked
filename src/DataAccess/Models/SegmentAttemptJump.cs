using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class SegmentAttemptJump
    {
        public int SegmentAttemptJumpId { get; set; }
        public int SegmentAttemptId { get; set; }
        public int JumpId { get; set; }
        public int Number { get; set; }

        public virtual Jump Jump { get; set; }
        public virtual SegmentAttempt SegmentAttempt { get; set; }
    }
}
