using Akela.Behaviours;
using Akela.Globals;
using UnityEngine;

namespace Akela.Motion
{
	[AddComponentMenu("Motion/Transform Driver", 1)]
	[TickOptions(TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.LateUpdate, TickUpdateType.AnimatorMove)]
	[ExecuteInEditMode]
	public class TransformDriver : TickBehaviour
	{
		public enum TransformProperty
		{
			LocalPositionX,
			LocalPositionY,
			LocalPositionZ,
			GlobalPositionX,
			GlobalPositionY,
			GlobalPositionZ,
			LocalRotationX,
			LocalRotationY,
			LocalRotationZ,
			GlobalRotationX,
			GlobalRotationY,
			GlobalRotationZ,
			ScaleX,
			ScaleY,
			ScaleZ
		}

		#region Component Fields
		[Header("Driver")]
		[SerializeField] Transform _drivingTransform;
		[SerializeField] TransformProperty _drivingProperty;
		[SerializeField] Vector2 _drivingLimits;

		[Header("Drivee")]
		[SerializeField] Vector3 _referenceValue;
		[SerializeField] TransformProperty _drivenProperty;
		[SerializeField] Vector2 _drivenLimits;
		[SerializeField] Var<AnimationCurve> _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		#endregion

		private Quaternion _referenceRotation;

		#region Component Messages
		private void Start()
		{
			_referenceRotation = Quaternion.Euler(_referenceValue);
		}

		protected override void Tick(float deltaTime)
		{
#if UNITY_EDITOR
			if (!didStart)
				Start();
#endif
			if (!_drivingTransform)
				return;

			var drivingValue = _drivingProperty switch
			{
				TransformProperty.LocalPositionX => _drivingTransform.localPosition.x,
				TransformProperty.LocalPositionY => _drivingTransform.localPosition.y,
				TransformProperty.LocalPositionZ => _drivingTransform.localPosition.z,
				TransformProperty.GlobalPositionX => _drivingTransform.position.x,
				TransformProperty.GlobalPositionY => _drivingTransform.position.y,
				TransformProperty.GlobalPositionZ => _drivingTransform.position.z,
				TransformProperty.LocalRotationX => _drivingTransform.localEulerAngles.x,
				TransformProperty.LocalRotationY => _drivingTransform.localEulerAngles.y,
				TransformProperty.LocalRotationZ => _drivingTransform.localEulerAngles.z,
				TransformProperty.GlobalRotationX => _drivingTransform.eulerAngles.x,
				TransformProperty.GlobalRotationY => _drivingTransform.eulerAngles.y,
				TransformProperty.GlobalRotationZ => _drivingTransform.eulerAngles.z,
				TransformProperty.ScaleX => _drivingTransform.localScale.x,
				TransformProperty.ScaleY => _drivingTransform.localScale.y,
				TransformProperty.ScaleZ => _drivingTransform.localScale.z,
				_ => 0f
			};

			drivingValue = Mathf.Clamp01((drivingValue - _drivingLimits.x) / (_drivingLimits.y - _drivingLimits.x));
			drivingValue = _curve.Value.Evaluate(drivingValue);

			var drivenValue = Mathf.Lerp(_drivenLimits.x, _drivenLimits.y, drivingValue);

			switch (_drivenProperty)
			{
				case TransformProperty.LocalPositionX: transform.localPosition = new(drivenValue, transform.localPosition.y, transform.localPosition.z); break;
				case TransformProperty.LocalPositionY: transform.localPosition = new(transform.localPosition.x, drivenValue, transform.localPosition.z); break;
				case TransformProperty.LocalPositionZ: transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, drivenValue); break;
				case TransformProperty.GlobalPositionX: transform.position = new(drivenValue, transform.position.y, transform.position.z); break;
				case TransformProperty.GlobalPositionY: transform.position = new(transform.position.x, drivenValue, transform.position.z); break;
				case TransformProperty.GlobalPositionZ: transform.position = new(transform.position.x, transform.position.y, drivenValue); break;
				case TransformProperty.LocalRotationX:
					transform.localRotation = _referenceRotation * Quaternion.AngleAxis(drivenValue, Vector3.right);
					break;
				case TransformProperty.LocalRotationY:
					transform.localRotation = _referenceRotation * Quaternion.AngleAxis(drivenValue, Vector3.up);
					break;
				case TransformProperty.LocalRotationZ:
					transform.localRotation = _referenceRotation * Quaternion.AngleAxis(drivenValue, Vector3.forward);
					break;
				case TransformProperty.GlobalRotationX: transform.rotation = Quaternion.Euler(drivenValue, transform.eulerAngles.y, transform.eulerAngles.z); break;
				case TransformProperty.GlobalRotationY: transform.rotation = Quaternion.Euler(transform.eulerAngles.x, drivenValue, transform.eulerAngles.z); break;
				case TransformProperty.GlobalRotationZ: transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, drivenValue); break;
				case TransformProperty.ScaleX: transform.localScale = new(drivenValue, transform.localScale.y, transform.localScale.z); break;
				case TransformProperty.ScaleY: transform.localScale = new(transform.localScale.x, drivenValue, transform.localScale.z); break;
				case TransformProperty.ScaleZ: transform.localScale = new(transform.localScale.x, transform.localScale.y, drivenValue); break;
			}
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_drivingLimits.x > _drivingLimits.y)
				_drivingLimits.y = _drivingLimits.x;

			if (_drivenLimits.x > _drivenLimits.y)
				_drivenLimits.y = _drivenLimits.x;
		}
#endif
		#endregion
	}
}
