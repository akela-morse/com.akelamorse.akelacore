//using System;
//using UltEvents;
//using UnityEngine;

//namespace SOL.Triggers
//{
//	public class CounterTrigger : MonoBehaviour, ITrigger
//	{
//		public enum CounterOperator
//		{
//			Equals,
//			GreaterThan,
//			LessThan
//		}

//		public event Action OnActivate;
//		public event Action OnDeactivate;

//		#region Component Fields
//		public int startValue;
//		public int targetValue;
//		public CounterOperator @operator;
//		public bool fireOnce;


//		[SerializeField] UltEvent _onReachTarget;

//		[SerializeField] UltEvent _onFailTarget;
//		#endregion

//		private int _value;
//		private bool _active;
//		private bool _fired;

//		public bool IsActive() => _active;

//		public void Increment()
//		{
//			_value++;

//			TestCounter();
//		}

//		public void Decrement()
//		{
//			if (_value > 0)
//				_value--;

//			TestCounter();
//		}

//		public void Set(int value)
//		{
//			_value = value;

//			TestCounter();
//		}

//		#region Component Messages
//		private void Start()
//		{
//			_value = startValue;
//			TestCounter();
//		}
//		#endregion

//		private void TestCounter()
//		{
//			var success = @operator switch
//			{
//				CounterOperator.Equals => _value == targetValue,
//				CounterOperator.GreaterThan => _value > targetValue,
//				CounterOperator.LessThan => _value < targetValue,
//				_ => false,
//			};

//			bool runEvents;

//			if (!success)
//			{
//				runEvents = _active;

//				_active = false;

//				if (runEvents)
//				{
//					OnDeactivate?.Invoke();
//					_onFailTarget.Invoke();
//				}
//			}
//			else if (!fireOnce || !_fired)
//			{
//				runEvents = !_active;

//				_fired = true;
//				_active = true;

//				if (runEvents)
//				{
//					OnActivate?.Invoke();
//					_onReachTarget.Invoke();
//				}
//			}
//		}
//	}
//}
