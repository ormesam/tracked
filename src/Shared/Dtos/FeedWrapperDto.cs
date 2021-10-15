using System.Collections.Generic;

namespace Shared.Dtos {
    public class FeedWrapperDto {
        public IList<RideFeedDto> Rides { get; set; }
        public IList<FollowFeedDto> Follows { get; set; }
    }
}
