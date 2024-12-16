using System;
using Akela.Bridges;
using UnityEngine;

namespace Akela.Triggers
{
    [AddComponentMenu("Triggers/Exit Trigger", -1)]
    [DisallowMultipleComponent]
    public class ExitTrigger : MonoBehaviour, ITrigger
    {
        #region Component Fields
        [SerializeField] BridgedEvent onDisable;
        [SerializeField] BridgedEvent onDestroy;
        #endregion

        public bool IsActive { get; private set; }

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType != TriggerEventType.OnBecomeActive)
                return;

            onDisable.AddListener(() => callback());
        }

        #region Component Messages
        private void OnDisable()
        {
            IsActive = true;

            onDisable.Invoke();
        }

        private void OnDestroy()
        {
            onDestroy.Invoke();
        }
        #endregion
    }
}