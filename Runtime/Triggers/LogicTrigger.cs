using System;
using System.Collections.Generic;
using Akela.Behaviours;
using Akela.Bridges;
using UnityEngine;

namespace Akela.Triggers
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/LogicTrigger Icon.png")]
    [AddComponentMenu("Triggers/Logic Trigger", 5)]
    public class LogicTrigger : MonoBehaviour, ITrigger, ISerializationCallbackReceiver
    {
        private enum LogicOperator
        {
            AND,
            OR,
            XOR
        }

        #region Component Fields
        [SerializeField] LogicOperator _operation;
        [SerializeField] bool _onlyTriggerOnStateChange = true;
        [SerializeField] List<MonoBehaviour> _members = new();
        [Header("Events")]
        [SerializeField] BridgedEvent _onActive;
        [SerializeField] BridgedEvent _onInactive;
        #endregion

        private List<ITrigger> _triggers = new();

        public bool IsActive { get; private set; }

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType == TriggerEventType.OnBecomeInactive)
                _onInactive.AddListener(callback);
            else
                _onActive.AddListener(callback);
        }

        #region ISerializationCallbackReceiver
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _members.Clear();

            foreach (var trigger in _triggers)
                _members.Add((MonoBehaviour)trigger);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _triggers.Clear();

            foreach (var member in _members)
                _triggers.Add((ITrigger)member);
        }
        #endregion

        #region Component Messages
        private void Awake()
        {
            foreach (var t in _triggers)
            {
                // ReSharper disable once RedundantArgumentDefaultValue
                t.AddListener(RefreshState, TriggerEventType.OnBecomeActive);
                t.AddListener(RefreshState, TriggerEventType.OnBecomeInactive);
            }

            RefreshState();
        }
        #endregion

        #region Private Methods
        private void RefreshState()
        {
            var state = _operation == LogicOperator.AND;

            foreach (var t in _triggers)
            {
                if (t == null)
                    continue;

                switch (_operation)
                {
                    case LogicOperator.AND:
                        state = state && t.IsActive;
                        break;

                    case LogicOperator.OR:
                        state = state || t.IsActive;
                        break;

                    case LogicOperator.XOR:
                        state = state != t.IsActive;
                        break;
                }
            }

            if (IsActive == state && _onlyTriggerOnStateChange)
                return;

            IsActive = state;

            if (state)
                _onActive.Invoke();
            else
                _onInactive.Invoke();
        }
        #endregion
    }
}