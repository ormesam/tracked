namespace Shared.Dtos {
    public class LoginResponseDto {
        public string AccessToken { get; set; }
        public UserDto User { get; set; }
    }
}
