using Akela.Bridges;
using UnityEngine;

namespace Akela.Signals
{
	[AddComponentMenu("Signals/Signal Relayer", 0)]
	public class SignalRelayer : MonoBehaviour, ISignalReceiver
	{
		#region Component Fields
		[SerializeField] Signal[] _signalsToListenFor;
		[Space]
		[SerializeField] BridgedEvent<Signal> _onSignalReceived;
		#endregion

		public Signal[] ListenFor => _signalsToListenFor;

		public void OnSignalReceived(Signal signal)
		{
			_onSignalReceived.Invoke(signal);
		}
	}
}