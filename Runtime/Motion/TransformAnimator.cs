using Akela.Behaviours;
using UnityEngine;

namespace Akela.Motion
{
    [HideScriptField, ExecuteInEditMode, DisallowMultipleComponent]
    [TickOptions(TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate, TickUpdateType.AnimatorMove)]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/TransformAnimator Icon.png")]
    [AddComponentMenu("Motion/Transform Animator", 3)]
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

        private sbyte _animationDirection = 1;

        public TransformAnimationPlayingState PlayingState { get; private set; } = TransformAnimationPlayingState.Stopped;
        public float Time { get; private set; }

        public TransformAnimation TransformAnimation => _transformAnimation;
        public float Duration => _transformAnimation.Duration();

#if UNITY_EDITOR
        public bool ControlledByEditor { get; set; }
#endif

        public void Play()
        {
            if (PlayingState == TransformAnimationPlayingState.Playing)
                return;

            PlayingState = TransformAnimationPlayingState.Playing;
        }

        public void ChangeDirection(bool reverse)
        {
            PlayingState = TransformAnimationPlayingState.Playing;

            _animationDirection = (sbyte)(reverse ? -1 : 1);
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
            Time = 0f;

            if (!_transformAnimation.IsValid)
                return;

            _transformAnimation.GetFirstKey(out var pos, out var rot, out var scale);
            transform.localPosition = pos;
            transform.localEulerAngles = rot;
            transform.localScale = scale;

            _animationDirection = 1;
        }

        public void SetPositionAtEnd()
        {
            Time = _transformAnimation.Duration();

            if (!_transformAnimation.IsValid)
                return;

            _transformAnimation.GetLastKey(out var pos, out var rot, out var scale);
            transform.localPosition = pos;
            transform.localEulerAngles = rot;
            transform.localScale = scale;

            if (_endState == TransformAnimationEndState.Reverse)
                _animationDirection = -1;
        }

        #region Component Messages
        private void Start()
        {
            if (!_transformAnimation || !_transformAnimation.IsValid)
                return;

            _transformAnimation.GetFirstKey(out var pos, out var rot, out var scale);
            transform.localPosition = pos;
            transform.localEulerAngles = rot;
            transform.localScale = scale;

#if UNITY_EDITOR
            if (_playOnStart && Application.isPlaying)
#else
            if (_playOnStart)
#endif
                Play();
        }

        protected override void Tick(float deltaTime)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && ControlledByEditor)
                Start();
#endif

            if (PlayingState != TransformAnimationPlayingState.Playing || !_transformAnimation || !_transformAnimation.IsValid)
                return;

            Time += _animationDirection * deltaTime * _speedMultiplier;

            var ended = !_transformAnimation.Evaluate(Time, out var pos, out var rot, out var scale);

            if (ended)
            {
                StopAnimation();
                return;
            }

            transform.localPosition = pos;
            transform.localEulerAngles = rot;
            transform.localScale = scale;
        }

#if UNITY_EDITOR
        private void OnRenderObject()
        {
            if (Application.isPlaying || PlayingState != TransformAnimationPlayingState.Playing || !_transformAnimation || !_transformAnimation.IsValid)
                return;

            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
#endif
        #endregion

        #region Private Methods
        private void StopAnimation()
        {
            if (!_loop)
                PlayingState = TransformAnimationPlayingState.Stopped;

            Vector3 pos, rot, scale;

            switch (_endState)
            {
                case TransformAnimationEndState.Stay:
                    _transformAnimation.GetLastKey(out pos, out rot, out scale);
                    transform.localPosition = pos;
                    transform.localEulerAngles = rot;
                    transform.localScale = scale;
                    break;

                case TransformAnimationEndState.Reset:
                    _transformAnimation.GetFirstKey(out pos, out rot, out scale);
                    transform.localPosition = pos;
                    transform.localEulerAngles = rot;
                    transform.localScale = scale;

                    Time = 0f;
                    break;

                case TransformAnimationEndState.Reverse:
                    if (_animationDirection > 0)
                    {
                        _transformAnimation.GetLastKey(out pos, out rot, out scale);
                        transform.localPosition = pos;
                        transform.localEulerAngles = rot;
                        transform.localScale = scale;

                        _animationDirection = -1;
                    }
                    else
                    {
                        _transformAnimation.GetFirstKey(out pos, out rot, out scale);
                        transform.localPosition = pos;
                        transform.localEulerAngles = rot;
                        transform.localScale = scale;

                        _animationDirection = 1;
                    }
                    break;
            }
        }
        #endregion
    }
}