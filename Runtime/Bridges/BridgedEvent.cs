using System;
using UnityEngine;

namespace Akela.Bridges
{
	[Serializable]
    public class BridgedEvent : IBridge
    {
#if AKELA_ULTEVENTS
		[SerializeField] UltEvents.UltEvent _internalValue;
#else
        [SerializeField] UnityEngine.Events.UnityEvent _internalValue;
#endif

        public void Invoke() => _internalValue.Invoke();
	}

	[Serializable]
	public class BridgedEvent<T0> : IBridge
	{
#if AKELA_ULTEVENTS
		[SerializeField] UltEvents.UltEvent<T0> _internalValue;
#else
        [SerializeField] UnityEngine.Events.UnityEvent<T0> _internalValue;
#endif

		public void Invoke(T0 arg0) => _internalValue.Invoke(arg0);
	}

	[Serializable]
	public class BridgedEvent<T0, T1> : IBridge
	{
#if AKELA_ULTEVENTS
		[SerializeField] UltEvents.UltEvent<T0, T1> _internalValue;
#else
        [SerializeField] UnityEngine.Events.UnityEvent<T0, T1> _internalValue;
#endif

		public void Invoke(T0 arg0, T1 arg1) => _internalValue.Invoke(arg0, arg1);
	}

	[Serializable]
	public class BridgedEvent<T0, T1, T2> : IBridge
	{
#if AKELA_ULTEVENTS
		[SerializeField] UltEvents.UltEvent<T0, T1, T2> _internalValue;
#else
        [SerializeField] UnityEngine.Events.UnityEvent<T0, T1, T2> _internalValue;
#endif

		public void Invoke(T0 arg0, T1 arg1, T2 arg2) => _internalValue.Invoke(arg0, arg1, arg2);
	}

	[Serializable]
	public class BridgedEvent<T0, T1, T2, T3> : IBridge
	{
#if AKELA_ULTEVENTS
		[SerializeField] UltEvents.UltEvent<T0, T1, T2, T3> _internalValue;
#else
        [SerializeField] UnityEngine.Events.UnityEvent<T0, T1, T2, T3> _internalValue;
#endif

		public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3) => _internalValue.Invoke(arg0, arg1, arg2, arg3);
	}
}
