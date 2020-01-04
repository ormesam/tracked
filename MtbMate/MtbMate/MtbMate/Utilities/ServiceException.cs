using System;

namespace MtbMate.Utilities {
    public class ServiceException : Exception {
        public ServiceException() : base() { }
        public ServiceException(string msg) : base(msg) { }
        public ServiceException(Exception exception) : base("", exception) { }
    }
}
