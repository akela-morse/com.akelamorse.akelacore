using System;
using Akela.Bridges;
using UnityEngine;

namespace Akela.Signals
{
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/SignalRelayer Icon.png")]
    [AddComponentMenu("Signals/Signal Relayer", 0)]
    public class SignalRelayer : MonoBehaviour, ISignalReceiver
    {
        #region Component Fields
        [SerializeField] Signal[] _signalsToListenFor;
        [Space]
        [SerializeField] BridgedEvent<Signal> _onSignalReceived;
        #endregion

        public string[] ListenFor => Array.ConvertAll(_signalsToListenFor, x => x.name);

        public void OnSignalReceived(Signal signal)
        {
            _onSignalReceived.Invoke(signal);
        }
    }
}