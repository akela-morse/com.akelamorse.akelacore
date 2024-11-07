#pragma warning disable UNT0014 // Need interface support
using UnityEngine;

namespace Akela.Events
{
    public class EventBroadcaster<T> where T: class
    {
        public delegate void ListenerEvent(T listener);

        private readonly T[] _listeners;

        public EventBroadcaster(GameObject containerObject)
        {
			_listeners = containerObject.GetComponents<T>();
		}

        public void Dispatch(ListenerEvent @event)
        {
            foreach (var listener in _listeners)
                @event(listener);
        }
    }
}
