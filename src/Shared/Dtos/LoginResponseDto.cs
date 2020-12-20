namespace Shared.Dtos {
    public class LoginResponseDto {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public UserDto User { get; set; }
    }
}
