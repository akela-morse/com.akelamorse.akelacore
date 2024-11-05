using Akela.Behaviours;
using Akela.Bridges;
using Akela.Globals;
using UnityEngine;

namespace SOL.Motion
{
	public abstract class ElevatorBase : TickBehaviour
	{
		#region Component Fields
		[SerializeField] protected Var<AnimationCurve> curve;
		[SerializeField] protected float time;
		[SerializeField] protected bool startAtUpPosition;

		[Space]

		[SerializeField] BridgedEvent _onStartMoving;
		[SerializeField] BridgedEvent _onStopMoving;
		#endregion

		private bool _moving;
		private bool _isUp;
		private float _animationProgression;
		private int _animationDirection;

		public void GoUp()
		{
			if (!enabled)
				return;

			if (_moving)
			{
				if (_animationDirection < 0)
					_animationDirection = 1;

				return;
			}

			if (_isUp)
				return;

			Move();
		}

		public void GoDown()
		{
			if (!enabled)
				return;

			if (_moving)
			{
				if (_animationDirection > 0)
					_animationDirection = -1;

				return;
			}

			if (!_isUp)
				return;

			Move();
		}

		public void GoToNextPos()
		{
			if (!enabled)
				return;

			Move();
		}

		public void TeleportUp()
		{
			if (!enabled)
				return;

			_isUp = true;
			Setup();

			if (_moving)
				_moving = false;
		}

		public void TeleportDown()
		{
			if (!enabled)
				return;

			_isUp = false;
			Setup();

			if (_moving)
				_moving = false;
		}

		#region Component Messages
		private void Awake()
		{
			_isUp = startAtUpPosition;

			Setup();
		}

		protected override void Tick(float deltaTime)
		{
			if (!_moving)
				return;

			_animationProgression = Mathf.Clamp01(_animationProgression + _animationDirection * (deltaTime / time));

			var value = curve.Value.Evaluate(_animationProgression);

			ApplyMotion(value);

			if (_animationDirection < 0 && value <= 0f || _animationDirection > 0 && value >= 1f)
			{
				_isUp = _animationDirection > 0;

				Setup();

				_onStopMoving.Invoke();

				_moving = false;
			}
		}
		#endregion

		#region Private Methods
		protected abstract void ApplyMotion(float value);
		protected abstract void SetToUpPos();
		protected abstract void SetToDownPos();

		private void Setup()
		{
			if (_isUp)
			{
				_animationProgression = 1f;
				_animationDirection = -1;

				SetToUpPos();
			}
			else
			{
				_animationProgression = 0f;
				_animationDirection = 1;

				SetToDownPos();
			}
		}

		private void Move()
		{
			if (_moving)
				return;

			_onStartMoving.Invoke();

			_moving = true;
		}
		#endregion
	}
}