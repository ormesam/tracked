using Shared.Dtos;

namespace Tracked.Auth {
    public class GoogleResponse {
        public GoogleUserDto User { get; set; }
        public GoogleActionStatus Status { get; set; }
        public string Message { get; set; }

        public GoogleResponse(GoogleClientResultEventArgs e) {
            User = e.User;
            Status = e.Status;
            Message = e.Message;
        }
    }
}
