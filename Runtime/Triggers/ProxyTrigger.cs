using System;
using Akela.Behaviours;
using UnityEngine;

namespace Akela.Triggers
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/ProxyTrigger Icon.png")]
    [AddComponentMenu("Triggers/Proxy Trigger", 11)]
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