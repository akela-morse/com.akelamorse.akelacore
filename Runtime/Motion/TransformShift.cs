using Akela.Behaviours;
using Akela.Globals;
using Akela.Tools;
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
        [SerializeField] private Vector3 _endPosition;
        [SerializeField, EulerAngles] private Quaternion _endRotation = Quaternion.identity;
        [SerializeField] private Vector3 _endScale = Vector3.one;
        [SerializeField] private Var<AnimationCurve> _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Motion Settings")]
        [SerializeField] private bool _playOnStart;
        [SerializeField] private float _motionTime = 1f;
        [SerializeField] private bool _loop;
        [SerializeField] private TransformAnimationEndState _endState;
        #endregion

        private sbyte _lerpDirection = 1;
        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private Vector3 _startScale;

        public Vector3 endPosition { get => _endPosition; set => _endPosition = value; }
        public Quaternion endRotation { get => _endRotation; set => _endRotation = value; }
        public Vector3 endScale { get => _endScale; set => _endScale = value; }
        public bool playOnStart { get => _playOnStart; set => _playOnStart = value; }
        public float motionTime { get => _motionTime; set => _motionTime = value; }

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

        public void ChangeDirection(bool reverse)
        {
            PlayingState = TransformAnimationPlayingState.Playing;

            _lerpDirection = (sbyte)(reverse ? -1 : 1);
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

        public void ResetStartPosition()
        {
            transform.GetLocalPositionAndRotation(out _startPosition, out _startRotation);

            _startScale = transform.localScale;
        }

        public void SetPositionAtStart()
        {
            transform.SetLocalPositionAndRotation(_startPosition, _startRotation);

            transform.localScale = _startScale;

            Progression = 0f;
            _lerpDirection = 1;
        }

        public void SetPositionAtEnd()
        {
            transform.SetLocalPositionAndRotation(_endPosition, _endRotation);

            transform.localScale = _endScale;

            Progression = 1f;

            if (_endState == TransformAnimationEndState.Reverse)
                _lerpDirection = -1;
        }

        #region Component Messages
        private void Awake()
        {
            ResetStartPosition();
        }

        private void Start()
        {
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

            transform.SetLocalPositionAndRotation(
                Vector3.LerpUnclamped(_startPosition, _endPosition, lerp),
                Quaternion.LerpUnclamped(_startRotation, _endRotation, lerp)
            );

            transform.localScale = Vector3.LerpUnclamped(_startScale, _endScale, lerp);

            if (_lerpDirection < 0 && Progression <= 0f || _lerpDirection > 0 && Progression >= 1f)
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
                    transform.SetLocalPositionAndRotation(_endPosition, _endRotation);
                    transform.localScale = _endScale;

                    Progression = 1f;
                    break;

                case TransformAnimationEndState.Reset:
                    transform.SetLocalPositionAndRotation(_startPosition, _startRotation);
                    transform.localScale = _startScale;

                    Progression = 0f;
                    break;

                case TransformAnimationEndState.Reverse:
                    if (_lerpDirection > 0)
                    {
                        transform.SetLocalPositionAndRotation(_endPosition, _endRotation);
                        transform.localScale = _endScale;

                        Progression = 1f;
                        _lerpDirection = -1;
                    }
                    else
                    {
                        transform.SetLocalPositionAndRotation(_startPosition, _startRotation);
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