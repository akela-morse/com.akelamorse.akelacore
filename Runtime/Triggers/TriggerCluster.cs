using System.Linq;
using UnityEngine;

namespace Akela.Triggers
{
	public class TriggerCluster
	{
		private readonly Collider[] _colliders;
		private Bounds _bounds;

        public TriggerCluster(GameObject obj)
        {
            _colliders = obj.GetComponentsInChildren<Collider>().Where(x => x.isTrigger).ToArray();

			RefreshBounds();
		}

		public void RefreshBounds()
		{
			_bounds = _colliders[0].bounds;

			for (var i = 1; i < _colliders.Length; i++)
				_bounds.Encapsulate(_colliders[i].bounds);
		}

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
	}
}
