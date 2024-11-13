using System;
using Akela.Behaviours;
using UnityEngine;

namespace Akela.Triggers
{
    [AddComponentMenu("Triggers/Proxy Trigger", 11)]
    [HideScriptField]
    public class ProxyTrigger : MonoBehaviour, ITrigger
    {
        [SerializeField] MonoBehaviour _proxy;

        public bool IsActive => ((ITrigger)_proxy).IsActive;

        public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
        {
            ((ITrigger)_proxy).AddListener(callback, eventType);
        }
    }
}
