using Akela.Behaviours;
using System.Linq;
using UnityEngine;

namespace Akela.Triggers
{
	[AddComponentMenu("Triggers/Trigger Cluster", 0)]
	[DisallowMultipleComponent]
	[TickOptions(TickUpdateType.None, TickUpdateType.Update, TickUpdateType.FixedUpdate)]
	public class TriggerCluster : TickBehaviour
	{
		private Collider[] _colliders;
		private Bounds _bounds;		
		
		public float SqrDistanceFromBounds(Vector3 point)
		{
			return _bounds.SqrDistance(point);
		}

		public bool Contains(Transform transform)
		{
			return Contains(transform.position);
		}

		public bool Contains(Vector3 point)
		{
			if (!_bounds.Contains(point))
				return false;

			foreach (var collider in _colliders)
			{
				if (collider.ClosestPoint(point) == point)
					return true;
			}

			return false;
		}

		public Vector3 ClosestFrom(Vector3 point)
		{
			var candidate = Vector3.zero;
			var minDistance = float.MaxValue;

			foreach (var c in _colliders)
			{
				var nearest = c.ClosestPoint(point);

				if (nearest == point)
					return point;

				var dist = Vector3.SqrMagnitude(point - nearest);

				if (dist < minDistance)
				{
					candidate = nearest;
					minDistance = dist;
				}
			}

			return candidate;
		}

		#region Component Messages
		private void Awake()
		{
			_colliders = GetComponentsInChildren<Collider>().Where(x => x.isTrigger).ToArray();

			RefreshBounds();
		}

		protected override void Tick(float deltaTime)
		{
			RefreshBounds();
		}
		#endregion

		#region Private Methods
		public void RefreshBounds()
		{
			_bounds = _colliders[0].bounds;

			for (var i = 1; i < _colliders.Length; i++)
				_bounds.Encapsulate(_colliders[i].bounds);
		}		
		#endregion
	}
}
