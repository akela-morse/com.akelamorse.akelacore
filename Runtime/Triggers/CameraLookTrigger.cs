using System;
using Akela.Bridges;
using UnityEngine;

namespace Akela.Triggers
{
    [AddComponentMenu("Triggers/Camera Look Trigger", 4)]
    public class CameraLookTrigger : MonoBehaviour, ITrigger
    {
        #region Component Fields
        [SerializeField] float _proximity = -1f;
        [SerializeField] float _angleThreshold = -1f;
        [SerializeField] bool _triggerOnlyOnce;
        [Header("Events")]
        [SerializeField] BridgedEvent _onActive;
        [SerializeField] BridgedEvent _onInactive;
        #endregion

        private Camera _camera;
        private bool _triggered;

        public bool IsActive { get; private set; }

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType == TriggerEventType.OnBecomeInactive)
                _onInactive.AddListener(() => callback());
            else
                _onActive.AddListener(() => callback());
        }

        public void ResetFiringState()
        {
            _triggered = false;
        }

        #region Component Messages
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_triggerOnlyOnce && _triggered)
                return;

            var state = true;

            if (_proximity >= 0f)
                state &= (transform.position - _camera.transform.position).sqrMagnitude <= _proximity * _proximity;

            if (_angleThreshold >= 0f)
                state &= Vector3.Angle(_camera.transform.forward, transform.position - _camera.transform.position) <= _angleThreshold;

            if (state == IsActive)
                return;

            IsActive = state;

            if (IsActive)
            {
                _triggered = true;
                
                _onActive.Invoke();
            }
            else
            {
                _onInactive.Invoke();
            }
        }
        #endregion
    }
}
