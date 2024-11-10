using Akela.Bridges;
using UnityEngine;

namespace Akela.Signals
{
	[AddComponentMenu("Signals/Signal Relayer", 0)]
	public class SignalRelayer : MonoBehaviour, ISignalReceiver
	{
		#region Component Fields
		[SerializeField] SignalType[] _eventTypesToListenFor;
		[Space]
		[SerializeField] BridgedEvent<Signal> _onEventReceived;
		#endregion

		public SignalType[] ListenFor => _eventTypesToListenFor;

		public void OnSignalReceived(Signal signal)
		{
			_onEventReceived.Invoke(signal);
		}
	}
}