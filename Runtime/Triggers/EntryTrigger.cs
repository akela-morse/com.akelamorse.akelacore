using System;
using Akela.Bridges;
using UnityEngine;

namespace Akela.Triggers
{
    [AddComponentMenu("Signals/Entry Trigger", -2)]
    [DisallowMultipleComponent]
    public class EntryTrigger : MonoBehaviour, ITrigger
    {
        #region Component Fields
        [SerializeField] BridgedEvent onAwake;
        [SerializeField] BridgedEvent onEnable;
        [SerializeField] BridgedEvent onStart;
        #endregion

        public bool IsActive { get; private set; }

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType != TriggerEventType.OnBecomeActive)
                return;

            onEnable.AddListener(() => callback());
        }

        #region Component Messages
        private void Awake()
        {
            onAwake.Invoke();
        }

        private void OnEnable()
        {
            IsActive = true;

            onEnable.Invoke();
        }

        private void Start()
        {
            onStart.Invoke();
        }
        #endregion
    }
}
