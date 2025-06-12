using System;
using Akela.Behaviours;
using Akela.Bridges;
using Akela.Globals;
using Akela.Tools;
using UnityEngine;

namespace Akela.Triggers
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/CollisionTrigger Icon.png")]
    [AddComponentMenu("Triggers/Collision Trigger", 2)]
    public class CollisionTrigger : MonoBehaviour, ITrigger
    {
        #region Component Fields
        [SerializeField] Var<LayerMask> _layerMask;
        [SerializeField] Var<string> _tag;
        [SerializeField] bool _triggerOnlyOnce;
        [Header("Events")]
        [SerializeField] BridgedEvent<Collision> _onEnter;
        [SerializeField] BridgedEvent<Collision> _onExit;
        [SerializeField] BridgedEvent<Collision> _onStay;
        #endregion

        private bool _triggered;

        public bool IsActive { get; private set; }

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            AddListener(_ => callback(), eventType);
        }

        public void AddListener(Action<Collision> callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType == TriggerEventType.OnBecomeInactive)
                _onExit.AddListener(callback);
            else
                _onEnter.AddListener(callback);
        }

        #region Component Messages
        private void OnCollisionEnter(Collision collision)
        {
            if (!CheckConditions(collision))
                return;

            _triggered = true;
            IsActive = true;

            _onEnter.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!CheckConditions(collision))
                return;

            IsActive = false;

            _onExit.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!CheckConditions(collision))
                return;

            _triggered = true;
            IsActive = true;

            _onStay.Invoke(collision);
        }
        #endregion

        #region Private Methods
        private bool CheckConditions(Collision collision)
        {
            var fireTest = !_triggerOnlyOnce || _triggerOnlyOnce && !_triggered;
            var tagTest = _tag != string.Empty || collision.gameObject.CompareTag(_tag);
            var layerTest = _layerMask.Value.Contains(collision.gameObject.layer);

            return fireTest && tagTest && layerTest;
        }
        #endregion
    }
}