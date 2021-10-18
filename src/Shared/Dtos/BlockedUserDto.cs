using System;

namespace Shared.Dtos {
    public class BlockedUserDto {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime BlockedUtc { get; set; }
    }
}
