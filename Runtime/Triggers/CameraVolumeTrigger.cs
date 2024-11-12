using System;
using Akela.Behaviours;
using Akela.Bridges;
using UnityEngine;

namespace Akela.Triggers
{
    [AddComponentMenu("Triggers/Camera Volume Trigger", 3)]
    [HideScriptField]
    [RequireComponent(typeof(TriggerCluster))]
    public class CameraVolumeTrigger : MonoBehaviour, ITrigger
    {
        #region Component Fields
        [SerializeField] bool _triggerOnlyOnce;
        [Header("Events")]
        [SerializeField] BridgedEvent _onActive;
        [SerializeField] BridgedEvent _onInactive;
        #endregion

        private Transform _camera;
        private TriggerCluster _triggerCluster;
        private bool _triggered;

        public bool IsActive { get; private set; }

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType == TriggerEventType.OnBecomeInactive)
                _onInactive.AddListener(() => callback());
            else
                _onActive.AddListener(() => callback());
        }

        #region Component Messages
        private void Awake()
        {
            _camera = Camera.main?.transform;
            _triggerCluster = GetComponent<TriggerCluster>();
        }

        private void Start()
        {
            IsActive = _triggered = _triggerCluster.Contains(_camera);
        }

        private void Update()
        {
            if (_triggered && _triggerOnlyOnce)
                return;

            var state = _triggerCluster.Contains(_camera);

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
