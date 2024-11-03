using Akela.Behaviours;
using Akela.Globals;
using UnityEngine;

namespace Akela.Optimisations
{
	public class CullingElement : TickBehaviour, ICullingElement
	{
		private const string MESSAGE_BECOME_VISIBLE = "OnCullingElementVisible";
		private const string MESSAGE_BECOME_INVISIBLE = "OnCullingElementInvisible";
		private const string MESSAGE_DISTANCE_CHANGE = "OnDistanceBandChanges";

		#region Component Fields
		[SerializeField] Var<CullingSystem> _system;
		[Space]
		[SerializeField] Vector3 _sphereCenter;
		[SerializeField] float _sphereRadius;
		#endregion

		private int _elementId;

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
				SendMessage(MESSAGE_BECOME_VISIBLE, SendMessageOptions.DontRequireReceiver);

			if (data.hasBecomeInvisible)
				SendMessage(MESSAGE_BECOME_INVISIBLE, SendMessageOptions.DontRequireReceiver);

			if (data.previousDistance != data.currentDistance)
				SendMessage(MESSAGE_DISTANCE_CHANGE, SendMessageOptions.DontRequireReceiver);
		}

		#region Component Messages
		private void Awake()
		{
			_elementId = _system.Value.RegisterElement(this, new(_sphereCenter.x, _sphereCenter.y, _sphereCenter.z, _sphereRadius));
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