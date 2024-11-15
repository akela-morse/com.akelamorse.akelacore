using System;
using UnityEngine;
#if AKELA_ULTEVENTS
using UltEvents;
#else
using UnityEngine.Events;
#endif

namespace Akela.Bridges
{
    [Serializable]
    public class BridgedEvent : IBridge
    {
#if AKELA_ULTEVENTS
        [SerializeField] UltEvent _internalValue;

        public void AddListener(Action callback) => _internalValue.AddListener(callback);
#else
        [SerializeField] UnityEvent _internalValue;

        public void AddListener(UnityAction callback) => _internalValue.AddListener(callback);
#endif

        public void Invoke() => _internalValue.Invoke();
    }

    [Serializable]
    public class BridgedEvent<T0> : IBridge
    {
#if AKELA_ULTEVENTS
        [SerializeField] UltEvent<T0> _internalValue;

        public void AddListener(Action<T0> callback) => _internalValue.AddListener(callback);
#else
        [SerializeField] UnityEvent<T0> _internalValue;

        public void AddListener(UnityAction<T0> callback) => _internalValue.AddListener(callback);
#endif

        public void Invoke(T0 arg0) => _internalValue.Invoke(arg0);
    }

    [Serializable]
    public class BridgedEvent<T0, T1> : IBridge
    {
#if AKELA_ULTEVENTS
        [SerializeField] UltEvent<T0, T1> _internalValue;

        public void AddListener(Action<T0, T1> callback) => _internalValue.AddListener(callback);
#else
        [SerializeField] UnityEvent<T0, T1> _internalValue;

        public void AddListener(UnityAction<T0, T1> callback) => _internalValue.AddListener(callback);
#endif

        public void Invoke(T0 arg0, T1 arg1) => _internalValue.Invoke(arg0, arg1);
    }

    [Serializable]
    public class BridgedEvent<T0, T1, T2> : IBridge
    {
#if AKELA_ULTEVENTS
        [SerializeField] UltEvent<T0, T1, T2> _internalValue;

        public void AddListener(Action<T0, T1, T2> callback) => _internalValue.AddListener(callback);
#else
        [SerializeField] UnityEvent<T0, T1, T2> _internalValue;

        public void AddListener(UnityAction<T0, T1, T2> callback) => _internalValue.AddListener(callback);
#endif

        public void Invoke(T0 arg0, T1 arg1, T2 arg2) => _internalValue.Invoke(arg0, arg1, arg2);
    }

    [Serializable]
    public class BridgedEvent<T0, T1, T2, T3> : IBridge
    {
#if AKELA_ULTEVENTS
        [SerializeField] UltEvent<T0, T1, T2, T3> _internalValue;

        public void AddListener(Action<T0, T1, T2, T3> callback) => _internalValue.AddListener(callback);
#else
        [SerializeField] UnityEvent<T0, T1, T2, T3> _internalValue;

        public void AddListener(UnityAction<T0, T1, T2, T3> callback) => _internalValue.AddListener(callback);
#endif

        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3) => _internalValue.Invoke(arg0, arg1, arg2, arg3);
    }
}
