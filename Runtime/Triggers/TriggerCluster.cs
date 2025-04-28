using System.Linq;
using Akela.Behaviours;
using UnityEngine;

namespace Akela.Triggers
{
    [DisallowMultipleComponent, HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/TriggerCluster Icon.png")]
    [AddComponentMenu("Triggers/Trigger Cluster", 0)]
    public class TriggerCluster : MonoBehaviour
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

        public void RefreshBounds()
        {
            _bounds = _colliders[0].bounds;

            for (var i = 1; i < _colliders.Length; ++i)
                _bounds.Encapsulate(_colliders[i].bounds);
        }

        #region Component Messages
        private void Awake()
        {
            _colliders = GetComponentsInChildren<Collider>().Where(x => x.isTrigger).ToArray();

            RefreshBounds();
        }
        #endregion
    }
}