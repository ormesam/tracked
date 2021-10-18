namespace Shared.Dtos {
    public class LoginDto {
        public string GoogleIdToken { get; set; }
        public GoogleUserDto User { get; set; }
    }
}
