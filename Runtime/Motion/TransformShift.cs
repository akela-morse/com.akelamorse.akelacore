using Akela.Behaviours;
using Akela.Globals;
using UnityEditor;
using UnityEngine;

namespace Akela.Motion
{
    [HideScriptField, ExecuteInEditMode, DisallowMultipleComponent]
    [TickOptions(TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate)]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/TransformLerp Icon.png")]
    [AddComponentMenu("Motion/Transform Shift", 2)]
    public class TransformShift : TickBehaviour
    {
        #region Component Fields
        [Space]
        [SerializeField] Vector3 _endPosition;
        [SerializeField] Vector3 _endRotation;
        [SerializeField] Vector3 _endScale = Vector3.one;
        [SerializeField] Var<AnimationCurve> _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Motion Settings")]
        public bool _playOnStart;
        [SerializeField] float _motionTime = 1f;
        [SerializeField] bool _loop;
        [SerializeField] TransformAnimationEndState _endState;
        #endregion

        private sbyte _lerpDirection = 1;
        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private Vector3 _startScale;

        public TransformAnimationPlayingState PlayingState { get; private set; } = TransformAnimationPlayingState.Stopped;
        public float Progression { get; private set; }

#if UNITY_EDITOR
        public bool ControlledByEditor { get; set; }
#endif

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
            transform.localEulerAngles = _startRotation;
            transform.localScale = _startScale;

            Progression = 0f;
            _lerpDirection = 1;
        }

        public void SetPositionAtEnd()
        {
            transform.localPosition = _endPosition;
            transform.localEulerAngles = _endRotation;
            transform.localScale = _endScale;

            Progression = 1f;

            if (_endState == TransformAnimationEndState.Reverse)
                _lerpDirection = -1;
        }

        #region Component Messages
        private void Start()
        {
            _startPosition = transform.localPosition;
            _startRotation = transform.localEulerAngles;
            _startScale = transform.localScale;
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
            if (!Application.isPlaying && !ControlledByEditor)
                Start();
#endif

            if (PlayingState != TransformAnimationPlayingState.Playing)
                return;

            Progression = Mathf.Clamp01(Progression + _lerpDirection * (deltaTime / _motionTime));

            var lerp = _curve.Value.Evaluate(Progression);

            transform.localPosition = Vector3.LerpUnclamped(_startPosition, _endPosition, lerp);
            transform.localEulerAngles = Vector3.LerpUnclamped(_startRotation, _endRotation, lerp);
            transform.localScale = Vector3.LerpUnclamped(_startScale, _endScale, lerp);

            if (_lerpDirection < 0 && lerp <= 0f || _lerpDirection > 0 && lerp >= 1f)
                StopAnimation();
        }

#if UNITY_EDITOR
        private void OnRenderObject()
        {
            if (Application.isPlaying || PlayingState != TransformAnimationPlayingState.Playing)
                return;

            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
        }
#endif
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
                    transform.localEulerAngles = _endRotation;
                    transform.localScale = _endScale;

                    Progression = 1f;
                    break;

                case TransformAnimationEndState.Reset:
                    transform.localPosition = _startPosition;
                    transform.localEulerAngles = _startRotation;
                    transform.localScale = _startScale;

                    Progression = 0f;
                    break;

                case TransformAnimationEndState.Reverse:
                    if (_lerpDirection > 0)
                    {
                        transform.localPosition = _endPosition;
                        transform.localEulerAngles = _endRotation;
                        transform.localScale = _endScale;

                        Progression = 1f;
                        _lerpDirection = -1;
                    }
                    else
                    {
                        transform.localPosition = _startPosition;
                        transform.localEulerAngles = _startRotation;
                        transform.localScale = _startScale;

                        Progression = 0f;
                        _lerpDirection = 1;
                    }
                    break;
            }
        }
        #endregion
    }
}