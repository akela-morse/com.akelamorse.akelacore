using System;
using Akela.Behaviours;
using Akela.Bridges;
using UnityEngine;
using UnityEngine.Serialization;

namespace Akela.Triggers
{
    [AddComponentMenu("Triggers/Counter Trigger", 8)]
    [HideScriptField]
    public class CounterTrigger : MonoBehaviour, ITrigger
    {
        private enum CounterOperator
        {
            Equals,
            GreaterThan,
            LessThan
        }

        #region Component Fields
        [SerializeField] int _startValue;
        [SerializeField] int _targetValue;
        [SerializeField] CounterOperator _checkOperation;
        [SerializeField] bool _triggerOnlyOnce = true;
        [Header("Events")]
        [SerializeField] BridgedEvent _onReachTarget;
        [SerializeField] BridgedEvent _onMissTarget;
        #endregion

        private int _value;
        private bool _triggered;

        public bool IsActive { get; private set; }

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType == TriggerEventType.OnBecomeInactive)
                _onMissTarget.AddListener(() => callback());
            else
                _onReachTarget.AddListener(() => callback());
        }

        public void Increment()
        {
            _value++;
            TestCounter();
        }

        public void Decrement()
        {
            if (_value > 0)
                _value--;

            TestCounter();
        }

        public void Set(int value)
        {
            _value = value;
            TestCounter();
        }

        #region Component Messages
        private void Start()
        {
            _value = _startValue;
            TestCounter();
        }
        #endregion

        #region Private Methods
        private void TestCounter()
        {
            var success = _checkOperation switch
            {
                CounterOperator.Equals => _value == _targetValue,
                CounterOperator.GreaterThan => _value > _targetValue,
                CounterOperator.LessThan => _value < _targetValue,
                _ => false,
            };

            bool runEvents;

            if (!success)
            {
                runEvents = IsActive;

                IsActive = false;

                if (runEvents)
                    _onMissTarget.Invoke();
            }
            else if (!_triggerOnlyOnce || !_triggered)
            {
                runEvents = !IsActive;

                _triggered = true;
                IsActive = true;

                if (runEvents)
                    _onReachTarget.Invoke();
            }
        }
        #endregion
    }
}
