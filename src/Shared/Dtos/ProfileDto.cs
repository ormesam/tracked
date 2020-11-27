using System;

namespace Shared.Dtos {
    public class ProfileDto {
        public string Name { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
