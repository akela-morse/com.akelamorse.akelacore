#if AKELA_CINEMACHINE && AKELA_INPUTSYSTEM
using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Akela.Cinemachine
{
    [SaveDuringPlay, ExecuteAlways]
    [Icon("Packages/com.unity.cinemachine/Editor/EditorResources/Icons/Light/CmExtension@256.png")]
    [AddComponentMenu("Cinemachine/Helpers/Cinemachine Gyroscopic Axis Controller")]
    public class CinemachineGyroscopicAxisController : InputAxisControllerBase<CinemachineGyroscopicAxisController.Reader>
    {
        #region Component Fields
        [SerializeField] private InputActionReference _gyroAction;
        [SerializeField] private InputActionReference _accelAction;
        #endregion

        [NonSerialized] private Vector3 _gyro;
        [NonSerialized] private Vector3 _accel;
        [NonSerialized] private Vector3 _gravity;

        [Serializable]
        public sealed class Reader : IInputAxisReader
        {
            const float yawRelaxFactor = 1.41f;

            [SerializeField] private float _sensitivity = 1f;

            public float GetValue(Object context, IInputAxisOwner.AxisDescriptor.Hints hint)
            {
                var controller = (CinemachineGyroscopicAxisController)context;

                if (hint == IInputAxisOwner.AxisDescriptor.Hints.Y)
                    return controller._gyro.x * _sensitivity * Mathf.Rad2Deg;

                if (hint == IInputAxisOwner.AxisDescriptor.Hints.X)
                {
                    var gravNorm = controller._gravity.normalized;
                    var worldYaw = controller._gyro.y * gravNorm.y + controller._gyro.z * gravNorm.z; // dot product but just yaw and roll

                    return -Mathf.Sign(worldYaw) *
                           Mathf.Min(
                               Mathf.Abs(worldYaw) * yawRelaxFactor,
                               new Vector2(controller._gyro.y, controller._gyro.z).magnitude
                           ) *
                           _sensitivity *
                           Mathf.Rad2Deg;
                }

                return 0f;
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            const float accelInfluence = .02f;

            UpdateControllers();

            var deltaTime = IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

            _gyro = _gyroAction.action.ReadValue<Vector3>();
            _accel = _accelAction.action.ReadValue<Vector3>();

            var rotation = Quaternion.AngleAxis(Vector3.Magnitude(_gyro) * deltaTime, -_gyro);

            // rotate gravity vector
            _gravity = rotation * _gravity;

            // nudge towards gravity according to current acceleration
            var newGravity = -_accel;
            _gravity += (newGravity - _gravity) * accelInfluence;
        }
    }
}
#endif