using System;
using Akela.Behaviours;
using Akela.Bridges;
using UnityEngine;

namespace Akela.Triggers
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/FlipFlopTrigger Icon.png")]
    [AddComponentMenu("Triggers/Flip-Flop Trigger", 10)]
    public class FlipFlopTrigger : MonoBehaviour, ITrigger
    {
        #region Component Fields
        [SerializeField] bool _startFlipped;
        [Header("Events")]
        [SerializeField] BridgedEvent _onFlipped;
        [SerializeField] BridgedEvent _onUnflipped;
        #endregion

        public bool IsFlipped { get; private set; }
        public bool IsActive { get; private set; }

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType == TriggerEventType.OnBecomeInactive)
                _onUnflipped.AddListener(callback);
            else
                _onFlipped.AddListener(callback);
        }

        public void Flip()
        {
            IsFlipped = !IsFlipped;

            if (IsFlipped)
                _onFlipped.Invoke();
            else
                _onUnflipped.Invoke();

            IsActive = true;
        }

        public void ForceState(bool flipped)
        {
            IsFlipped = flipped;

            if (IsFlipped)
                _onFlipped.Invoke();
            else
                _onUnflipped.Invoke();
        }

        #region Component Fields
        private void OnEnable()
        {
            if (_startFlipped)
                ForceState(true);
        }
        #endregion
    }
}