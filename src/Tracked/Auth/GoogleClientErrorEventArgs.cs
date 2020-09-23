using System;

namespace Tracked.Auth {
    public class GoogleClientErrorEventArgs : EventArgs {
        public GoogleClientErrorType Error { get; set; }
        public string Message { get; set; }
    }
}
