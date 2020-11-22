using System;

namespace Shared.Dtos {
    public class UserDto {
        public int? UserId { get; init; }
        public string Name { get; init; }
        public string ProfileImageUrl { get; init; }
        public bool IsAdmin { get; init; }
        public DateTime CreatedUtc { get; init; }
    }
}
