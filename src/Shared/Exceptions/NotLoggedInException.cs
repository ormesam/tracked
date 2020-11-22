using System;
using System.Runtime.Serialization;

namespace Shared.Exceptions {
    [Serializable]
    public class NotLoggedInException : Exception {
        public NotLoggedInException() {
        }

        public NotLoggedInException(string message) : base(message) {
        }

        public NotLoggedInException(string message, Exception innerException) : base(message, innerException) {
        }

        protected NotLoggedInException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}