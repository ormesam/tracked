using System;
using Shared.Dto;

namespace Tracked.Auth {
    public class GoogleClientResultEventArgs : EventArgs {
        public GoogleUserDto User { get; set; }
        public GoogleActionStatus Status { get; set; }
        public string Message { get; set; }

        public GoogleClientResultEventArgs(GoogleUserDto user, GoogleActionStatus status, string msg = "") {
            User = user;
            Status = status;
            Message = msg;
        }
    }
}
