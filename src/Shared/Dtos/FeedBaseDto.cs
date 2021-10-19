using System;
using Essentials.Core.Extensions;

namespace Shared.Dtos {
    public class FeedBaseDto {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public DateTime Date { get; set; }

        public string TimeDisplay => Date.ToReadableString();
    }
}
