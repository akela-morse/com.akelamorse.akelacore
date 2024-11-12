using Akela.Bridges;
using Akela.Globals;
using Akela.Tools;
using System;
using UnityEngine;

namespace Akela.Triggers
{
	[AddComponentMenu("Triggers/Volume Trigger", 1)]
	public class VolumeTrigger : MonoBehaviour, ITrigger
	{
		#region Component Fields
		[Header("Parameters")]
		[SerializeField] Var<LayerMask> _layerMask;
		[SerializeField] Var<string> _tag;
		[SerializeField] bool _triggerOnlyOnce;
		[Header("Events")]
		[SerializeField] BridgedEvent<Collider> _onEnter;
		[SerializeField] BridgedEvent<Collider> _onExit;
		[SerializeField] BridgedEvent<Collider> _onStay;
		#endregion

		private bool _triggered = false;
		private bool _active = false;

		public bool IsActive => _active;

		public void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive)
		{
			if (eventType == TriggerEventType.OnBecomeInactive)
				_onExit.AddListener(_ => callback());
			else
				_onEnter.AddListener(_ => callback());
		}

		public void ResetFiringState()
		{
			_triggered = false;
		}

		#region Component Messages
		private void OnTriggerEnter(Collider other)
		{
			if (!CheckConditions(other))
				return;

			_triggered = true;
			_active = true;

			_onEnter.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!CheckConditions(other))
				return;

			_active = false;

			_onExit.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			if (!CheckConditions(other))
				return;

			_triggered = true;
			_active = true;

			_onStay.Invoke(other);
		}
		#endregion

		#region Private Methods
		private bool CheckConditions(Collider other)
		{
			var fireTest = !_triggerOnlyOnce || _triggerOnlyOnce && !_triggered;
			var tagTest = string.IsNullOrEmpty(_tag) || other.gameObject.CompareTag(_tag);
			var layerTest = _layerMask.Value.Contains(other.gameObject.layer);

			return fireTest && tagTest && layerTest;
		}
		#endregion
	}
}