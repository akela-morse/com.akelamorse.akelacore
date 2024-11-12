//using SOL.Utilities;
//using System;
//using UltEvents;
//using UnityEngine;

//namespace SOL.Triggers
//{
//	public class IntervalTrigger : MonoBehaviour, ITrigger
//	{
//		public event Action OnActivate;
//		public event Action OnDeactivate { add { } remove { } } // unused in this context

//		#region Component Fields
//		public float interval;
//		public float maxActivationDistance = 100f;

//		[SerializeField] UltEvent _onInterval;
//		#endregion

//		private bool _active;
//		private float _time;

//		public bool IsActive() => _active;

//		#region Component Methods
//		private void OnEnable()
//		{
//			_time = Time.time;
//		}

//		private void Update()
//		{
//			if (Vector3.Distance(Camera.main.transform.position, transform.position) >= maxActivationDistance)
//				return;

//			_active = (Time.time - _time) >= interval;

//			if (_active)
//			{
//				OnActivate?.Invoke();
//				_onInterval.Invoke();

//				_time = Time.time;
//			}
//		}
//		#endregion
//	}
//}
