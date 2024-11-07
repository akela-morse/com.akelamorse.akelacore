using Akela.Behaviours;
using Akela.Events;
using Akela.Globals;
using UnityEngine;

namespace Akela.Optimisations
{
	[AddComponentMenu("Optimisation/Culling Element", 1)]
	public class CullingElement : TickBehaviour, ICullingElement
	{
		#region Component Fields
		[SerializeField] Var<CullingSystem> _system;
		[Space]
		[SerializeField] Vector3 _sphereCenter;
		[SerializeField] float _sphereRadius;
		#endregion

		private int _elementId;
		private EventBroadcaster<ICullingEventReceiver> _eventBroadcaster;

		public bool IsVisible { get; set; }
		public int CurrentDistanceBand { get; set; }

		public CullingSystem CullingSystem => _system;

		public void IndexChanged(int index)
		{
			_elementId = index;
		}

		public void StateChanged(CullingGroupEvent data)
		{
			IsVisible = data.isVisible;
			CurrentDistanceBand = data.currentDistance;

			if (data.hasBecomeVisible)
				_eventBroadcaster.Dispatch(x => x.OnCullingElementVisible());

			if (data.hasBecomeInvisible)
				_eventBroadcaster.Dispatch(x => x.OnCullingElementInvisible());

			if (data.previousDistance != data.currentDistance)
				_eventBroadcaster.Dispatch(x => x.OnDistanceBandChanges(data.previousDistance, data.currentDistance));
		}

		#region Component Messages
		private void Awake()
		{
			_elementId = _system.Value.RegisterElement(this, new(_sphereCenter.x, _sphereCenter.y, _sphereCenter.z, _sphereRadius));
			_eventBroadcaster = new(gameObject);
		}

		protected override void Tick(float deltaTime)
		{
			_system.Value.UpdateSpherePosition(_elementId, transform.TransformPoint(_sphereCenter));
		}

		private void OnDestroy()
		{
			_system.Value.UnregisterElement(_elementId);
		}

#if UNITY_EDITOR
		private void Reset()
		{
			_sphereRadius = 2f;
		}

		private void OnValidate()
		{
			if (_sphereRadius < 0f)
				_sphereRadius = 0f;
		}
#endif
		#endregion
	}
}