//using SOL.Utilities;
//using System;
//using System.Collections;
//using UltEvents;
//using UnityEngine;

//namespace SOL.Triggers
//{
//	public class DelayTrigger : MonoBehaviour, ITrigger
//	{
//		public event Action OnActivate;
//		public event Action OnDeactivate { add { } remove { } } // unused in this context

//		public float time;
//		public bool fireImmediately;

//		[SerializeField] UltEvent _onTimer;

//		private Coroutine coroutine;
//		private bool active = false;

//		public bool IsActive() => active;

//		private void OnEnable()
//		{
//			if (fireImmediately)
//				StartTimer();
//		}

//		public void StartTimer()
//		{
//			if (time <= 0f)
//			{
//				active = true;
//				OnActivate?.Invoke();
//				_onTimer.Invoke();

//				return;
//			}

//			coroutine = StartCoroutine(Timer());
//		}

//		public void StopTimer()
//		{
//			if (coroutine != null)
//				StopCoroutine(coroutine);
//		}

//		private IEnumerator Timer()
//		{
//			yield return new WaitForSeconds(time);
//			active = true;
//			OnActivate?.Invoke();
//			_onTimer.Invoke();
//		}
//	}
//}