//using AkelaTools;
//using SOL.Gameplay;
//using System;
//using UltEvents;
//using UnityEngine;

//namespace SOL.Triggers
//{
//	[RequireComponent(typeof(Collider))]
//	public class CameraVolumeTrigger : MonoBehaviour, ITrigger
//	{
//		public event Action OnActivate;
//		public event Action OnDeactivate;

//		#region Component Fields
//		public bool fireOnce;

//		[Header("Events")]

//		[SerializeField] UltEvent _onActive;
//		[SerializeField] UltEvent _onInactive;
//		#endregion

//		private Transform _camera;
//		private TriggerCluster _triggerCluster;
//		private bool _fired = false;
//		private bool _active = false;

//		public bool IsActive() => _active;

//		#region Component Messages
//		private void Awake()
//		{
//			_camera = Camera.main.transform;
//		}

//		private void Start()
//		{
//			_triggerCluster = new(gameObject);
//			_active = _fired = _triggerCluster.Contains(_camera);
//		}

//		private void Update()
//		{
//			if (!GameManager.Main.GameplayActive || (_fired && fireOnce))
//				return;

//			var state = _triggerCluster.Contains(_camera);

//			if (state == _active)
//				return;

//			_active = state;

//			if (_active)
//			{
//				_fired = true;
//				OnActivate?.Invoke();
//				_onActive.Invoke();
//			}
//			else
//			{
//				OnDeactivate?.Invoke();
//				_onInactive.Invoke();
//			}
//		}
//		#endregion
//	}
//}
