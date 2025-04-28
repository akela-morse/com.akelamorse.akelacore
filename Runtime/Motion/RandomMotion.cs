using Akela.Behaviours;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Akela.Motion
{
    [DisallowMultipleComponent]
    [TickOptions(TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate, TickUpdateType.AnimatorMove)]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/RandomMotion Icon.png")]
    [AddComponentMenu("Motion/Random Motion", 5)]
    public class RandomMotion : TickBehaviour
    {
        #region Component Fields
        [SerializeField] float _radius = 1f;
        [SerializeField] float _speed = 1f;
        [SerializeField] float _elasticity = 5f;
        [Range(0f, 1f)]
        [SerializeField] float _damping = 0.5f;
        [SerializeField] float _frequency = 1f;
        #endregion

        private Vector3 _anchorPos;
        private Vector3 _currentTarget;
        private Vector3 _curentDestination;
        private Vector3 _currentMotion;
        private float _jitterTime;

        #region Component Messages
        private void Start()
        {
            _anchorPos = transform.localPosition;
        }

        protected override void Tick(float deltaTime)
        {
            SetPos(deltaTime);
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_radius < 0f)
                _radius = 0f;
        }
#endif
        #endregion

        private void SetPos(float deltaTime)
        {
            var time = _updateType == TickUpdateType.FixedUpdate ? Time.fixedTime : Time.time;

            if (time - _jitterTime > 1f / _frequency)
            {
                _currentTarget = Random.onUnitSphere * _radius;
                _jitterTime = time;
            }

            _curentDestination = Vector3.MoveTowards(_curentDestination, _currentTarget, deltaTime * _elasticity);

            var toDestination = Vector3.ClampMagnitude(_anchorPos + _curentDestination - transform.localPosition, 1f) * _speed;

            if (_damping == 0f)
                _currentMotion = toDestination;
            else
                _currentMotion = Vector3.Lerp(_currentMotion, toDestination, deltaTime / _damping);

            transform.Translate(_currentMotion);
        }
    }
}