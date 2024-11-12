using System.Collections.Generic;
using System.Linq;
using Akela.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Akela.Signals
{
	[CreateAssetMenu(fileName = "New Signal", menuName = "Signal", order = -50)]
	public class Signal : ScriptableObject
	{
		private readonly List<ISignalReceiver> _listeners = new();

		public object Payload { get; private set; }

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
			ComponentLoader<ISignalReceiver>.OnTypeFound += RefreshListenersList;
		}

		private void RefreshListenersList(IEnumerable<ISignalReceiver> receivers)
		{
			_listeners.Clear();
			_listeners.AddRange(receivers.Where(x => x.ListenFor.Contains(this)));
		}

		private void DispatchToListeners<T>(T payload)
		{
			Payload = payload;

			foreach (var listener in _listeners)
				listener.OnSignalReceived(this);
		}

		public static implicit operator string(Signal signal) => signal.name;
	}
}
