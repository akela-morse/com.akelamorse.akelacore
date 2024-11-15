#pragma warning disable UNT0014 // Invalid type for call to GetComponent - Reason: Need interface support
using UnityEngine;

namespace Akela.Signals
{
    public class MessageBroadcaster<T> where T : class
    {
        public delegate void Message(T listener);

        private readonly T[] _listeners;

        public MessageBroadcaster(GameObject containerObject)
        {
            _listeners = containerObject.GetComponentsInParent<T>();
        }

        public void Dispatch(Message message)
        {
            foreach (var listener in _listeners)
            {
                if (listener == null)
                    continue;

                message(listener);
            }
        }
    }
}
