//using SOL.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UltEvents;
//using UnityEngine;

//namespace SOL.Triggers
//{
//	public class LogicTrigger : MonoBehaviour, ITrigger
//	{
//		public enum LogicOperator
//		{
//			AND,
//			OR,
//			XOR
//		}

//		public event Action OnActivate;
//		public event Action OnDeactivate;

//		public LogicOperator operation;
//		public List<MonoBehaviour> members;
//		public bool onlyFireOnStateChange = true;

//		[Header("Events")]

//		[SerializeField] UltEvent _onConditionMeet;

//		[SerializeField] UltEvent _onConditionUnmeet;

//		private bool state = false;
//		private List<ITrigger> _triggers;

//		public bool IsActive() => state;

//		private void Awake()
//		{
//			_triggers = members.Cast<ITrigger>().ToList();
//			_triggers.ForEach(x =>
//			{
//				x.OnActivate += RefreshState;
//				x.OnDeactivate += RefreshState;
//			});

//			RefreshState();
//		}

//		private void OnDestroy()
//		{
//			_triggers.ForEach(x =>
//			{
//				x.OnActivate -= RefreshState;
//				x.OnDeactivate -= RefreshState;
//			});
//		}

//		private void RefreshState()
//		{
//			var allOk = operation == LogicOperator.AND;

//			foreach (ITrigger t in _triggers)
//			{
//				if (t == null)
//					continue;

//				switch (operation)
//				{
//					case LogicOperator.AND:
//						allOk = allOk && t.IsActive();
//						break;

//					case LogicOperator.OR:
//						allOk = allOk || t.IsActive();
//						break;

//					case LogicOperator.XOR:
//						allOk = allOk != t.IsActive();
//						break;
//				}
//			}

//			if (state == allOk && onlyFireOnStateChange)
//				return;
				
//			state = allOk;

//			if (allOk)
//			{
//				OnActivate?.Invoke();
//				_onConditionMeet.Invoke();
//			}
//			else
//			{
//				OnDeactivate?.Invoke();
//				_onConditionUnmeet.Invoke();
//			}
//		}
//	}
//}