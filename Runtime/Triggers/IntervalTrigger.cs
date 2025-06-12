using System;
using Akela.Behaviours;
using Akela.Bridges;
using UnityEngine;

namespace Akela.Triggers
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/IntervalTrigger Icon.png")]
    [AddComponentMenu("Triggers/Interval Trigger", 7)]
    public class IntervalTrigger : MonoBehaviour, ITrigger
    {
        #region Component Fields
        [SerializeField] float _interval = 1f;
        [Header("Events")]
        [SerializeField] BridgedEvent _onInterval;
        #endregion

        private float _time;

        public bool IsActive { get; private set; }

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType != TriggerEventType.OnBecomeActive)
                return;

            _onInterval.AddListener(callback);
        }

        #region Component Methods
        private void OnEnable()
        {
            _time = Time.time;
        }

        private void Update()
        {
            IsActive = (Time.time - _time) >= _interval;

            if (IsActive)
            {
                _onInterval.Invoke();
                _time = Time.time;
            }
        }
        #endregion
    }
}