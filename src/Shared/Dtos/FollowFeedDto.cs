namespace Shared.Dtos {
    public class FollowFeedDto : FeedBaseDto {
        public int FollowedUserId { get; set; }
        public string FollowedUserName { get; set; }
        public string FollowedUserProfileImageUrl { get; set; }
    }
}
