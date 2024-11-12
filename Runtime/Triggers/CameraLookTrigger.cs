using Akela.Bridges;
using System;
using UnityEngine;

namespace Akela.Triggers
{
	[AddComponentMenu("Triggers/Camera Look Trigger", 4)]
	public class CameraLookTrigger : MonoBehaviour, ITrigger
	{
		#region Component Fields
		[Header("Parameters")]
		[SerializeField] float _proximity = -1f;
		[SerializeField] float _angleThreshold = -1f;
		[Header("Events")]
		[SerializeField] BridgedEvent _onConditionsMeet;
		[SerializeField] BridgedEvent _onConditionsUnmeet;
		#endregion

		private Camera _camera;
		private bool _active;

		public bool IsActive => _active;

		public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
		{
			if (eventType == TriggerEventType.OnBecomeInactive)
				_onConditionsUnmeet.AddListener(callback);
			else
				_onConditionsMeet.AddListener(callback);
		}

		#region Component Messages
		private void Awake()
		{
			_camera = Camera.main;
		}

		private void LateUpdate()
		{
			var ok = true;

			if (_proximity >= 0f)
				ok &= (transform.position - _camera.transform.position).sqrMagnitude <= _proximity * _proximity;

			if (_angleThreshold >= 0f)
				ok &= Vector3.Angle(_camera.transform.forward, transform.position - _camera.transform.position) <= _angleThreshold;

			if (ok != _active)
			{
				_active = ok;

				if (_active)
					_onConditionsMeet.Invoke();
				else
					_onConditionsUnmeet.Invoke();
			}
		}
		#endregion
	}
}
