using System;
using Akela.Behaviours;
using Akela.Bridges;
using Akela.Globals;
using Akela.Tools;
using UnityEngine;

namespace Akela.Triggers
{
    [AddComponentMenu("Triggers/Collision Trigger", 2)]
    [HideScriptField]
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
            if (eventType == TriggerEventType.OnBecomeActive)
                _onExit.AddListener(_ => callback());
            else
                _onEnter.AddListener(_ => callback());
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
            var tagTest = _tag.HasValue || collision.gameObject.CompareTag(_tag);
            var layerTest = _layerMask.Value.Contains(collision.gameObject.layer);

            return fireTest && tagTest && layerTest;
        }
        #endregion
    }
}
