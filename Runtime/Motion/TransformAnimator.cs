using Akela.Behaviours;
using UnityEngine;

namespace Akela.Motion
{
    [AddComponentMenu("Motion/Transform Animator", 3)]
    [TickOptions(TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate, TickUpdateType.AnimatorMove)]
    [HideScriptField, DisallowMultipleComponent]
    public class TransformAnimator : TickBehaviour
    {
        #region Component Fields
        [SerializeField] TransformAnimation _transformAnimation;
        [Header("Animation Settings")]
        public bool _playOnStart;
        [SerializeField] float _speedMultiplier = 1f;
        [SerializeField] bool _loop;
        [SerializeField] TransformAnimationEndState _endState;
        #endregion

        private float _animationTime;
        private sbyte _animationDirection = 1;

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
            _animationTime = 0f;

            _transformAnimation.GetFirstKey(out var pos, out var rot, out var scale);
            transform.SetLocalPositionAndRotation(pos, rot);
            transform.localScale = scale;

            _animationDirection = 1;
        }

        public void SetPositionAtEnd()
        {
            _animationTime = _transformAnimation.Duration();

            _transformAnimation.GetLastKey(out var pos, out var rot, out var scale);
            transform.SetLocalPositionAndRotation(pos, rot);
            transform.localScale = scale;

            if (_endState == TransformAnimationEndState.Reverse)
                _animationDirection = -1;
        }

        #region Component Messages
        private void Start()
        {
            _transformAnimation.GetFirstKey(out var pos, out var rot, out var scale);
            transform.SetLocalPositionAndRotation(pos, rot);
            transform.localScale = scale;

            if (_playOnStart)
                Play();
        }

        protected override void Tick(float deltaTime)
        {
            if (PlayingState != TransformAnimationPlayingState.Playing)
                return;

            _animationTime += _animationDirection * deltaTime * _speedMultiplier;

            var ended = !_transformAnimation.Evaluate(_animationTime, out var pos, out var rot, out var scale);

            if (ended)
            {
                StopAnimation();
                return;
            }

            transform.SetLocalPositionAndRotation(pos, rot);
            transform.localScale = scale;
        }
        #endregion

        #region Private Methods
        private void StopAnimation()
        {
            if (!_loop)
                PlayingState = TransformAnimationPlayingState.Stopped;

            Vector3 pos, scale;
            Quaternion rot;

            switch (_endState)
            {
                case TransformAnimationEndState.Stay:
                    _transformAnimation.GetLastKey(out pos, out rot, out scale);
                    transform.SetLocalPositionAndRotation(pos, rot);
                    transform.localScale = scale;
                    break;

                case TransformAnimationEndState.Reset:
                    _transformAnimation.GetFirstKey(out pos, out rot, out scale);
                    transform.SetLocalPositionAndRotation(pos, rot);
                    transform.localScale = scale;

                    _animationTime = 0f;
                    break;

                case TransformAnimationEndState.Reverse:
                    if (_animationDirection > 0)
                    {
                        _transformAnimation.GetLastKey(out pos, out rot, out scale);
                        transform.SetLocalPositionAndRotation(pos, rot);
                        transform.localScale = scale;

                        _animationDirection = -1;
                    }
                    else
                    {
                        _transformAnimation.GetFirstKey(out pos, out rot, out scale);
                        transform.SetLocalPositionAndRotation(pos, rot);
                        transform.localScale = scale;

                        _animationDirection = 1;
                    }
                    break;
            }
        }
        #endregion
    }
}
