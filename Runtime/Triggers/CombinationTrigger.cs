using System;
using System.Linq;
using Akela.Behaviours;
using Akela.Bridges;
using Akela.Tools;
using UnityEngine;

namespace Akela.Triggers
{
    [AddComponentMenu("Triggers/Combination Trigger", 9)]
    [HideScriptField]
    public class CombinationTrigger : MonoBehaviour, ITrigger
    {
        #region Component Fields
        [SerializeField] string _expectedCombination;
        [SerializeField] bool _bothWays;
        [Header("Events")]
        [SerializeField] BridgedEvent _onCombinationCorrect;
        [SerializeField] BridgedEvent _onCombinationIncorrect;
        #endregion

        private string _currentCombination = string.Empty;

        public bool IsActive { get; private set; }
        
        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            if (eventType == TriggerEventType.OnBecomeInactive)
                _onCombinationIncorrect.AddListener(() => callback());
            else
                _onCombinationCorrect.AddListener(() => callback());
        }

        public void EnterNewMember(string member)
        {
            _currentCombination += member;

            if (_currentCombination.Length >= _expectedCombination.Length)
            {
                var state = _currentCombination == _expectedCombination;

                if (!state && _bothWays)
                    state = (string)_currentCombination.Reverse() == _expectedCombination;

                if (state)
                {
                    IsActive = true;
                    _onCombinationCorrect.Invoke();
                }
                else
                {
                    IsActive = false;
                    _onCombinationIncorrect.Invoke();
                }
            }
        }

        public void ResetCombination()
        {
            _currentCombination = string.Empty;
            IsActive = false;
        }

        #region Component Messages
        private void Awake()
        {
            ResetCombination();
        }
        #endregion
    }
}
