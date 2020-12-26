using System;

namespace Tracked.Utilities {
    public class ServiceException : Exception {
        public ServiceExceptionType Type { get; }

        public ServiceException(string msg, ServiceExceptionType type) : base(msg) => Type = type;
    }

    public enum ServiceExceptionType {
        NotFound,
        ServerError,
        Unauthorized,
        UnableToConnect,
        Unknown,
        BadRequest,
    }
}
