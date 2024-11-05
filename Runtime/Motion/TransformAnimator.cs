using UnityEngine;

namespace SOL.Motion
{
	public class TransformAnimator : MonoBehaviour
	{
		public enum AnimationUpdateType
		{
			Normal,
			Late,
			Physics
		}

		public enum AnimationEndState
		{
			Stay,
			Reset,
			Reverse
		}

		public enum AnimationPlayingState
		{
			Stopped,
			Playing,
			Paused
		}

		#region Component Fields
		public Transform animatedTransform;
		public AnimationUpdateType animationUpdate;
		public bool useUnscaledTime;

		[Space]
		public Vector3 endPosition;
		public Vector3 endRotation;
		public Vector3 endScale = Vector3.one;
		public AnimationCurve curve;

		[Space]
		public bool playOnStart;
		public bool loop;
		public AnimationEndState endState;
		public float time = 1f;
		#endregion

		private float _animationProgression;
		private float _animationDirection = 1f;
		private AnimationPlayingState _playingState = AnimationPlayingState.Stopped;
		private Vector3 _startPosition;
		private Quaternion _startRotation;
		private Quaternion _endRotation;
		private Vector3 _startScale;

		public AnimationPlayingState PlayingState => _playingState;

		public void Play()
		{
			if (_playingState == AnimationPlayingState.Playing)
				return;

			_playingState = AnimationPlayingState.Playing;
		}

		public void Stop()
		{
			if (_playingState == AnimationPlayingState.Stopped)
				return;

			_playingState = AnimationPlayingState.Stopped;
			StopAnimation();
		}

		public void Pause()
		{
			if (_playingState != AnimationPlayingState.Playing)
				return;

			_playingState = AnimationPlayingState.Paused;
		}

		public void SetPositionAtStart()
		{
			animatedTransform.localPosition = _startPosition;
			animatedTransform.localRotation = _startRotation;
			animatedTransform.localScale = _startScale;

			_animationProgression = 0f;
			_animationDirection = 1f;
		}

		public void SetPositionAtEnd()
		{
			animatedTransform.localPosition = endPosition;
			animatedTransform.localRotation = _endRotation;
			animatedTransform.localScale = endScale;

			_animationProgression = 1f;

			if (endState == AnimationEndState.Reverse)
				_animationDirection = -1f;
		}

		#region Component Messages
		private void Start()
		{
			if (!animatedTransform)
				animatedTransform = transform;

			_startPosition = animatedTransform.localPosition;
			_startRotation = animatedTransform.localRotation;
			_startScale = animatedTransform.localScale;

			_endRotation = Quaternion.Euler(endRotation);

			if (playOnStart)
				Play();
		}

		private void Update()
		{
			if (animationUpdate != AnimationUpdateType.Normal || _playingState != AnimationPlayingState.Playing)
				return;

			Move(useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
		}

		private void LateUpdate()
		{
			if (animationUpdate != AnimationUpdateType.Late || _playingState != AnimationPlayingState.Playing)
				return;

			Move(useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
		}

		private void FixedUpdate()
		{
			if (animationUpdate != AnimationUpdateType.Physics || _playingState != AnimationPlayingState.Playing)
				return;

			Move(useUnscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime);
		}
		#endregion

		#region Private Methods
		private void Move(float deltaTime)
		{
			_animationProgression = Mathf.Clamp01(_animationProgression + _animationDirection * (deltaTime / time));

			var lerp = curve.Evaluate(_animationProgression);

			animatedTransform.localPosition = Vector3.LerpUnclamped(_startPosition, endPosition, lerp);
			animatedTransform.localRotation = Quaternion.LerpUnclamped(_startRotation, _endRotation, lerp);
			animatedTransform.localScale = Vector3.LerpUnclamped(_startScale, endScale, lerp);

			if (_animationDirection < 0f && lerp <= 0f || _animationDirection > 0f && lerp >= 1f)
				StopAnimation();
		}

		private void StopAnimation()
		{
			if (!loop)
				_playingState = AnimationPlayingState.Stopped;

			switch (endState)
			{
				case AnimationEndState.Stay:
					animatedTransform.localPosition = endPosition;
					animatedTransform.localRotation = _endRotation;
					animatedTransform.localScale = endScale;
					break;

				case AnimationEndState.Reset:
					animatedTransform.localPosition = _startPosition;
					animatedTransform.localRotation = _startRotation;
					animatedTransform.localScale = _startScale;

					_animationProgression = 0f;
					break;

				case AnimationEndState.Reverse:
					if (_animationDirection > 0f)
					{
						animatedTransform.localPosition = endPosition;
						animatedTransform.localRotation = _endRotation;
						animatedTransform.localScale = endScale;

						_animationDirection = -1f;
					}
					else
					{
						animatedTransform.localPosition = _startPosition;
						animatedTransform.localRotation = _startRotation;
						animatedTransform.localScale = _startScale;

						_animationDirection = 1f;
					}
					break;
			}
		}
		#endregion
	}
}
