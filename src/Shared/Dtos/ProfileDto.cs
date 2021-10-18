﻿using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class ProfileDto {
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public double? MilesTravelled { get; set; }
        public double? MilesTravelledThisMonth { get; set; }
        public double? TopSpeedMph { get; set; }
        public double? LongestAirtime { get; set; }
        public IList<AchievementDto> Achievements { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsFollowingCurrentUser { get; set; }
    }
}
