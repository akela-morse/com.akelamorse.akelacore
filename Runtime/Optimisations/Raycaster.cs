using Akela.Behaviours;
using Akela.Globals;
using Akela.Tools;
using UnityEngine;

namespace Akela
{
	[AddComponentMenu("Optimisation/Raycaster", 0)]
	[TickOptions(TickUpdateType.None, TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate, TickUpdateType.AnimatorMove)]
	public class Raycaster : TickBehaviour
	{
		public enum RaycastShape
		{
			Ray,
			Sphere,
			Box,
			Capsule
		}

		#region Component Fields
		[SerializeField] float _maxDistance = Mathf.Infinity;
		[SerializeField] Var<LayerMask> _layerMask;
		[SerializeField] QueryTriggerInteraction _triggerInteraction;
		[SerializeField] bool _registerMultipleHits;
		[SerializeField] int _maxNumberOfHits = 1;
		[SerializeField] RaycastShape _shape;
		[SerializeField] float _radius = 1f;
		[SerializeField, EulerAngles] Quaternion _orientation = Quaternion.identity;
		[SerializeField] Vector3 _boxSize = Vector3.one;
		[SerializeField] float _capsuleHeight = 2f;
		#endregion

		private RaycastHit[] _hits;
		private int _numberOfHits;

		#region Component Messages
		private void Awake()
		{
			_hits = new RaycastHit[_registerMultipleHits ? _maxNumberOfHits : 1];
		}

		protected override void Tick(float deltaTime)
		{
			PerformRaycast();
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			if (!didAwake)
				Awake();

			PerformRaycast();

			var ray = new Ray(transform.position, transform.forward);

			Gizmos.color = new(.1f, .55f, 1f);

			if (_maxDistance == Mathf.Infinity)
				GizmosHelper.DrawArrow(ray.origin, ray.direction, 1f);
			else
				GizmosHelper.DrawDottedLine(ray.origin, ray.origin + ray.direction * _maxDistance, 4f);

			if (_numberOfHits > 0)
			{
				Gizmos.color = new(1f, .4f, .1f);

				var furthestPoint = Vector3.zero;
				var longestDist = float.MinValue;

				for (var i = 0; i < _numberOfHits; ++i)
				{
					Gizmos.DrawSphere(_hits[i].point, .1f);

					var point = ray.GetPoint(_hits[i].distance);
					var sqrDist = (point - ray.origin).sqrMagnitude;

					if (longestDist < sqrDist)
					{
						longestDist = sqrDist;
						furthestPoint = point;
					}
				}

				GizmosHelper.DrawThickLine(ray.origin, furthestPoint, 2f);
			}

			switch (_shape)
			{
				case RaycastShape.Sphere:
					Gizmos.color = new(.85f, .67f, 1f);
					Gizmos.DrawWireSphere(ray.origin, _radius);

					if (_numberOfHits > 0)
					{
						Gizmos.color = new(1f, .67f, .1f);

						for (var i = 0; i < _numberOfHits; ++i)
							Gizmos.DrawWireSphere(ray.GetPoint(_hits[i].distance), _radius);
					}
					break;

				case RaycastShape.Box:
					Gizmos.color = new(.85f, .67f, 1f);
					Gizmos.matrix = Matrix4x4.TRS(ray.origin, _orientation, Vector3.one);
					Gizmos.DrawWireCube(Vector3.zero, _boxSize);

					if (_numberOfHits > 0)
					{
						Gizmos.color = new(1f, .67f, .1f);

						for (var i = 0; i < _numberOfHits; ++i)
						{
							Gizmos.matrix = Matrix4x4.TRS(ray.GetPoint(_hits[i].distance), _orientation, Vector3.one);
							Gizmos.DrawWireCube(Vector3.zero, _boxSize);
						}
					}
					break;

				case RaycastShape.Capsule:
					Gizmos.color = new(.85f, .67f, 1f);
					Gizmos.matrix = Matrix4x4.TRS(ray.origin, _orientation, Vector3.one);
					GizmosHelper.DrawWireCapsule(Vector3.zero, Vector3.up, _radius, _capsuleHeight);

					if (_numberOfHits > 0)
					{
						Gizmos.color = new(1f, .67f, .1f);

						for (var i = 0; i < _numberOfHits; ++i)
						{
							Gizmos.matrix = Matrix4x4.TRS(ray.GetPoint(_hits[i].distance), _orientation, Vector3.one);
							GizmosHelper.DrawWireCapsule(Vector3.zero, Vector3.up, _radius, _capsuleHeight);
						}
					}
					break;
			}
		}

		private void Reset()
		{
			_layerMask = (LayerMask)LayerMask.GetMask("Default");
		}

		private void OnValidate()
		{
			if (_maxDistance < 0f)
				_maxDistance = 0f;

			if (!_registerMultipleHits)
				_maxNumberOfHits = 1;

			if (_maxNumberOfHits < 1)
				_maxNumberOfHits = 1;

			if (_radius < 0f)
				_radius = 0f;

			if (_capsuleHeight < _radius * 2f)
				_capsuleHeight = _radius * 2f;
		}
#endif
		#endregion

		#region Private Methods
		private void PerformRaycast()
		{
			var ray = new Ray(transform.position, transform.forward);

			switch (_shape)
			{
				case RaycastShape.Ray:
					if (!_registerMultipleHits)
						_numberOfHits = Physics.Raycast(ray, out _hits[0], _maxDistance, _layerMask.Value, _triggerInteraction) ? 1 : 0;
					else
						_numberOfHits = Physics.RaycastNonAlloc(ray, _hits, _maxDistance, _layerMask.Value, _triggerInteraction);
					break;

				case RaycastShape.Sphere:
					if (!_registerMultipleHits)
						_numberOfHits = Physics.SphereCast(ray, _radius, out _hits[0], _maxDistance, _layerMask.Value, _triggerInteraction) ? 1 : 0;
					else
						_numberOfHits = Physics.SphereCastNonAlloc(ray, _radius, _hits, _maxDistance, _layerMask.Value, _triggerInteraction);
					break;

				case RaycastShape.Box:
					if (!_registerMultipleHits)
						_numberOfHits = Physics.BoxCast(ray.origin, _boxSize * .5f, ray.direction, out _hits[0], _orientation, _maxDistance, _layerMask.Value, _triggerInteraction) ? 1 : 0;
					else
						_numberOfHits = Physics.BoxCastNonAlloc(ray.origin, _boxSize * .5f, ray.direction, _hits, _orientation, _maxDistance, _layerMask.Value, _triggerInteraction);
					break;

				case RaycastShape.Capsule:
					var dir = _orientation * Vector3.up;
					var offset = (_capsuleHeight - (_radius * 2f)) * .5f;

					var p1 = ray.origin - dir * offset;
					var p2 = ray.origin + dir * offset;

					if (!_registerMultipleHits)
						_numberOfHits = Physics.CapsuleCast(p1, p2, _radius, ray.direction, out _hits[0], _maxDistance, _layerMask.Value, _triggerInteraction) ? 1 : 0;
					else
						_numberOfHits = Physics.CapsuleCastNonAlloc(p1, p2, _radius, ray.direction, _hits, _maxDistance, _layerMask.Value, _triggerInteraction);
					break;
			}
		}
		#endregion
	}
}
