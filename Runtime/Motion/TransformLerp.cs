using Akela.Behaviours;
using Akela.Globals;
using Akela.Tools;
using UnityEngine;

namespace Akela.Motion
{
    [AddComponentMenu("Motion/Transform Lerp", 2)]
    [TickOptions(TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate)]
    [HideScriptField]
    public class TransformLerp : TickBehaviour
    {
        #region Component Fields
        [Space]
        [SerializeField] Vector3 _endPosition;
        [SerializeField, EulerAngles] Quaternion _endRotation;
        [SerializeField] Vector3 _endScale = Vector3.one;
        [SerializeField] Var<AnimationCurve> _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Lerp Settings")]
        public bool _playOnStart;
        [SerializeField] float _lerpTime = 1f;
        [SerializeField] bool _loop;
        [SerializeField] TransformAnimationEndState _endState;
        #endregion

        private float _lerpProgression;
        private sbyte _lerpDirection = 1;
        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private Vector3 _startScale;

        public TransformAnimationPlayingState PlayingState { get; private set; } = TransformAnimationPlayingState.Stopped;

        public void Play()
        {
            if (PlayingState == TransformAnimationPlayingState.Playing)
                return;

            PlayingState = TransformAnimationPlayingState.Playing;
        }

        public void Stop()
        {
            if (PlayingState == TransformAnimationPlayingState.Stopped)
                return;

            PlayingState = TransformAnimationPlayingState.Stopped;

            StopAnimation();
        }

        public void Pause()
        {
            if (PlayingState != TransformAnimationPlayingState.Playing)
                return;

            PlayingState = TransformAnimationPlayingState.Paused;
        }

        public void SetPositionAtStart()
        {
            transform.localPosition = _startPosition;
            transform.localRotation = _startRotation;
            transform.localScale = _startScale;

            _lerpProgression = 0f;
            _lerpDirection = 1;
        }

        public void SetPositionAtEnd()
        {
            transform.localPosition = _endPosition;
            transform.localRotation = _endRotation;
            transform.localScale = _endScale;

            _lerpProgression = 1f;

            if (_endState == TransformAnimationEndState.Reverse)
                _lerpDirection = -1;
        }

        #region Component Messages
        private void Start()
        {
            _startPosition = transform.localPosition;
            _startRotation = transform.localRotation;
            _startScale = transform.localScale;

            if (_playOnStart)
                Play();
        }

        protected override void Tick(float deltaTime)
        {
            if (PlayingState != TransformAnimationPlayingState.Playing)
                return;

            _lerpProgression = Mathf.Clamp01(_lerpProgression + _lerpDirection * (deltaTime / _lerpTime));

            var lerp = _curve.Value.Evaluate(_lerpProgression);

            transform.localPosition = Vector3.LerpUnclamped(_startPosition, _endPosition, lerp);
            transform.localRotation = Quaternion.LerpUnclamped(_startRotation, _endRotation, lerp);
            transform.localScale = Vector3.LerpUnclamped(_startScale, _endScale, lerp);

            if (_lerpDirection < 0 && lerp <= 0f || _lerpDirection > 0 && lerp >= 1f)
                StopAnimation();
        }
        #endregion

        #region Private Methods
        private void StopAnimation()
        {
            if (!_loop)
                PlayingState = TransformAnimationPlayingState.Stopped;

            switch (_endState)
            {
                case TransformAnimationEndState.Stay:
                    transform.localPosition = _endPosition;
                    transform.localRotation = _endRotation;
                    transform.localScale = _endScale;
                    break;

                case TransformAnimationEndState.Reset:
                    transform.localPosition = _startPosition;
                    transform.localRotation = _startRotation;
                    transform.localScale = _startScale;

                    _lerpProgression = 0f;
                    break;

                case TransformAnimationEndState.Reverse:
                    if (_lerpDirection > 0)
                    {
                        transform.localPosition = _endPosition;
                        transform.localRotation = _endRotation;
                        transform.localScale = _endScale;

                        _lerpDirection = -1;
                    }
                    else
                    {
                        transform.localPosition = _startPosition;
                        transform.localRotation = _startRotation;
                        transform.localScale = _startScale;

                        _lerpDirection = 1;
                    }
                    break;
            }
        }
        #endregion
    }
}
