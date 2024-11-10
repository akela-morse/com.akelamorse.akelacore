using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Akela.Signals
{
	[CreateAssetMenu(fileName = "New Event", menuName = "Event", order = -50)]
	public class Signal : ScriptableObject
	{
		private ISignalReceiver[] _listeners;

		public object Payload { get; private set; }
		public SignalType Type => name;

		public void Dispatch() => DispatchToListeners<object>(null);

		public void Dispatch(int payload) => DispatchToListeners(payload);

		public void Dispatch(float payload) => DispatchToListeners(payload);

		public void Dispatch(string payload) => DispatchToListeners(payload);

		public void Dispatch(Vector2 payload) => DispatchToListeners(payload);

		public void Dispatch(Vector3 payload) => DispatchToListeners(payload);

		public void Dispatch(Vector4 payload) => DispatchToListeners(payload);

		public void Dispatch(Quaternion payload) => DispatchToListeners(payload);

		public void Dispatch(Object payload) => DispatchToListeners(payload);

		private void OnEnable()
		{
			_listeners = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None)
				.OfType<ISignalReceiver>()
				.Where(x => x.ListenFor.Contains(Type))
				.ToArray();
		}

		private void DispatchToListeners<T>(T payload)
		{
			Payload = payload;

			foreach (var listener in _listeners)
				listener.OnSignalReceived(this);
		}
	}
}
