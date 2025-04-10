using UnityEngine;

namespace Akela.Promises
{
    public static class PromiseFactory
    {
        public static Promise<T> NewPromise<T>(this MonoBehaviour mb, Promise<T>.ExecutionDelegate execution)
        {
            var newPromise = new Promise<T>();
            newPromise.Start(mb.StartCoroutine(execution(newPromise.Resolve, newPromise.Reject)));

            return newPromise;
        }

        public static void AbortPromise<T>(this MonoBehaviour mb, Promise<T> promise)
        {
            mb.StopCoroutine(promise.Coroutine);
        }

        public static Promise<T> Then<T>(this Promise<T> p, Promise<T>.ResolveDelegate handle)
        {
            p.Then += handle;
            return p;
        }

        public static Promise<T> Catch<T>(this Promise<T> p, Promise<T>.RejectDelegate handle)
        {
            p.Catch += handle;
            return p;
        }

        public static Promise<T> Finally<T>(this Promise<T> p, Promise<T>.FinalDelegate handle)
        {
            p.Finally += handle;
            return p;
        }
    }
}