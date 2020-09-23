using Shared.Dto;

namespace Shared.Dtos {
    public class LoginDto {
        public string GoogleAccessToken { get; set; }
        public GoogleUserDto User { get; set; }
    }
}
