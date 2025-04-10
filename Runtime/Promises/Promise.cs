using System.Collections;
using UnityEngine;

namespace Akela.Promises
{
    public sealed class Promise<T> : CustomYieldInstruction
    {
        public delegate void ResolveDelegate(T result);

        public delegate void RejectDelegate(object reason);

        public delegate void FinalDelegate();

        public delegate IEnumerator ExecutionDelegate(ResolveDelegate resolve, RejectDelegate reject);

        public event ResolveDelegate Then;
        public event RejectDelegate Catch;
        public event FinalDelegate Finally;

        private bool _keepWaiting;
        public override bool keepWaiting => _keepWaiting;

        internal Coroutine Coroutine { get; private set; }

        public T Result { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsRejected { get; private set; }

        internal Promise() { }

        internal void Start(Coroutine coroutine)
        {
            _keepWaiting = true;
            Coroutine = coroutine;
        }

        internal void Resolve(T result)
        {
            _keepWaiting = false;

            Result = result;
            IsCompleted = true;

            if (Then != null)
                Then(result);

            if (Finally != null)
                Finally();
        }

        internal void Reject(object reason)
        {
            _keepWaiting = false;

            IsRejected = true;

            if (Catch != null)
            {
                Catch(reason);
            }
#if UNITY_EDITOR
            else
            {
                var ex = new UnhandledPromiseRejectionException<T>(this);
                Debug.LogException(ex);
            }
#endif

            if (Finally != null)
                Finally();
        }
    }
}