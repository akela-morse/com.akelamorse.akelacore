using Akela.Behaviours;
using Akela.Globals;
using Akela.Signals;
using Akela.Tools;
using UnityEngine;

namespace Akela.ExtendedPhysics
{
    [AddComponentMenu("Physics/Raycaster", 100)]
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
        [SerializeField] Vector3 _direction = Vector3.forward;
        [SerializeField] Space _castSpace = Space.Self;
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
        private bool _previousRaycastDidHit;
        private MessageBroadcaster<IRaycastMessageReceiver> _messageBroadcaster;

        public int NumberOfHits => _numberOfHits;

        public Quaternion Orientation { get => _orientation; set => _orientation = value; }
        public float MaxDistance { get => _maxDistance; set => _maxDistance = value; }
        public Vector3 Direction { get => _castSpace == Space.Self ? transform.TransformDirection(_direction) : _direction; set => _direction = value.normalized; }

        public void RaycastNow(bool sendEvents = true)
        {
            DoRaycast();

            if (sendEvents)
                CheckRaycastResult();
            else
                _previousRaycastDidHit = RaycastDidHit();
        }

        public void RaycastNow(Ray ray, bool sendEvents = true)
        {
            DoRaycast(ray);

            if (sendEvents)
                CheckRaycastResult();
            else
                _previousRaycastDidHit = RaycastDidHit();
        }

        public bool RaycastDidHit()
        {
            return _numberOfHits > 0;
        }

        public bool RaycastDidHit(out RaycastHit hit)
        {
            if (!RaycastDidHit())
            {
                hit = default;
                return false;
            }

            hit = _hits[0];
            return true;
        }

        public RaycastHit GetHit(int index)
        {
            return _hits[index];
        }

        #region Component Messages
        private void Awake()
        {
            _hits = new RaycastHit[_registerMultipleHits ? _maxNumberOfHits : 1];
            _messageBroadcaster = new(gameObject);
        }

        protected override void Tick(float deltaTime)
        {
            DoRaycast();
            CheckRaycastResult();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!didAwake)
                Awake();

            if (!Application.isPlaying)
                DoRaycast();

            var ray = new Ray(transform.position, Direction);

            Gizmos.color = new(.33f, .85f, 1f);

            if (float.IsPositiveInfinity(_maxDistance))
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
                    Gizmos.DrawSphere(_hits[i].point, .08f);

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
                    Gizmos.color = new(.33f, .85f, 1f);
                    Gizmos.DrawWireSphere(ray.origin, _radius);

                    if (_numberOfHits > 0)
                    {
                        Gizmos.color = new(1f, .67f, .1f);

                        for (var i = 0; i < _numberOfHits; ++i)
                            Gizmos.DrawWireSphere(ray.GetPoint(_hits[i].distance), _radius);
                    }
                    break;

                case RaycastShape.Box:
                    Gizmos.color = new(.33f, .85f, 1f);
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
                    Gizmos.color = new(.33f, .85f, 1f);
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
        private void DoRaycast()
        {
            DoRaycast(new Ray(transform.position, Direction));
        }

        private void DoRaycast(Ray ray)
        {
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
                    var offset = (_capsuleHeight - _radius * 2f) * .5f;

                    var p1 = ray.origin - dir * offset;
                    var p2 = ray.origin + dir * offset;

                    if (!_registerMultipleHits)
                        _numberOfHits = Physics.CapsuleCast(p1, p2, _radius, ray.direction, out _hits[0], _maxDistance, _layerMask.Value, _triggerInteraction) ? 1 : 0;
                    else
                        _numberOfHits = Physics.CapsuleCastNonAlloc(p1, p2, _radius, ray.direction, _hits, _maxDistance, _layerMask.Value, _triggerInteraction);
                    break;
            }
        }

        private void CheckRaycastResult()
        {
            var raycastDidHit = RaycastDidHit();

            if (raycastDidHit && !_previousRaycastDidHit)
                _messageBroadcaster.Dispatch(x => x.OnRaycastHit());

            _previousRaycastDidHit = raycastDidHit;
        }
        #endregion
    }
}