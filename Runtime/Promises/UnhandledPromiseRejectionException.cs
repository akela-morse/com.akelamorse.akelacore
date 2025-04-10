using System;

namespace Akela.Promises
{
    public class UnhandledPromiseRejectionException<T> : Exception
    {
        public Promise<T> Promise { get; private set; }

        public UnhandledPromiseRejectionException(Promise<T> promise)
        {
            Promise = promise;
        }

        public UnhandledPromiseRejectionException(Promise<T> promise, string message) : base(message)
        {
            Promise = promise;
        }

        public UnhandledPromiseRejectionException(Promise<T> promise, string message, Exception inner) : base(message, inner)
        {
            Promise = promise;
        }
    }
}