//using AkelaTools;
//using SOL.Utilities;
//using System;
//using UltEvents;
//using UnityEngine;

//namespace SOL.Triggers
//{
//	public class CollisionTrigger : MonoBehaviour, ITrigger
//	{
//		public event Action OnActivate;
//		public event Action OnDeactivate;

//		public LayerMask layerMask = int.MaxValue;
//		public bool fireOnce;

//		[Space]

//		[SerializeField] UltEvent<Collision> _onEnter;

//		[SerializeField] UltEvent<Collision> _onExit;

//		[SerializeField] UltEvent<Collision> _onStay;

//		private bool fired = false;
//		private bool active = false;

//		public bool IsActive() => active;

//		private void OnCollisionEnter(Collision collision)
//		{
//			var fireTest = !fireOnce || fireOnce && !fired;
//			var layerTest = layerMask.Contains(collision.gameObject.layer);

//			if (fireTest && layerTest)
//			{
//				fired = true;
//				active = true;
//				OnActivate?.Invoke();
//				_onEnter.Invoke(collision);
//			}
//		}

//		private void OnCollisionExit(Collision collision)
//		{
//			var fireTest = !fireOnce || fireOnce && !fired;
//			var layerTest = layerMask.Contains(collision.gameObject.layer);

//			if (fireTest && layerTest)
//			{
//				active = false;
//				OnDeactivate?.Invoke();
//				_onExit.Invoke(collision);
//			}
//		}

//		private void OnCollisionStay(Collision collision)
//		{
//			var fireTest = !fireOnce || fireOnce && !fired;
//			var layerTest = layerMask.Contains(collision.gameObject.layer);

//			if (fireTest && layerTest)
//			{
//				fired = true;
//				active = true;
//				OnActivate?.Invoke();
//				_onStay.Invoke(collision);
//			}
//		}
//	}
//}