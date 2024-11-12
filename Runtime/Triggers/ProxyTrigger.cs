//using MalbersAnimations;
//using System;
//using UnityEngine;

//namespace SOL.Triggers
//{
//	public class ProxyTrigger : MonoBehaviour, ITrigger
//	{
//		public event Action OnActivate
//		{
//			add => ((ITrigger)proxy).OnActivate += value;
//			remove => ((ITrigger)proxy).OnActivate -= value;
//		}

//		public event Action OnDeactivate
//		{
//			add => ((ITrigger)proxy).OnDeactivate += value;
//			remove => ((ITrigger)proxy).OnDeactivate -= value;
//		}

//		[EnforceType(typeof(ITrigger))]
//		public MonoBehaviour proxy;

//		public bool IsActive() => ((ITrigger)proxy).IsActive();
//	}
//}