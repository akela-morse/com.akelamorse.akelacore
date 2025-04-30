#if AKELA_CINEMACHINE
using Akela.Tools;
using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using UnityEngine;

namespace Akela.Cinemachine
{
    /// <summary>
    /// This is a wrapper around the internal <see cref="Unity.Cinemachine.TargetTracking.Tracker"/> strcut from Cinemachine.
    /// It uses reflection to create delegate methods on a custom object for maximum efficiency.
    /// It can be used to create custom Virtual Camera Components
    /// </summary>
    [InternalWrapper(typeof(TrackerSettings), "Unity.Cinemachine.TargetTracking.Tracker")]
    public sealed partial class Tracker
    {
        [InternalMethod]
        private delegate void InitStateInfoDelegate(CinemachineComponentBase component, float deltaTime, BindingMode bindingMode, Vector3 up);

        [InternalMethod]
        private delegate void TrackTargetDelegate(CinemachineComponentBase component, float deltaTime, Vector3 up, Vector3 desiredCameraOffset, in TrackerSettings settings, out Vector3 outTargetPosition, out Quaternion outTargetOrient);

        [InternalMethod]
        private delegate Vector3 GetOffsetForMinimumTargetDistanceDelegate(CinemachineComponentBase component, Vector3 dampedTargetPos, Vector3 cameraOffset, Vector3 cameraFwd, Vector3 up, Vector3 actualTargetPos);

        [InternalMethod]
        private delegate void OnTargetObjectWarpedDelegate(Vector3 positionDelta);

        [InternalMethod]
        private delegate void ForceCameraPositionDelegate(CinemachineComponentBase component, BindingMode bindingMode, Vector3 pos, Quaternion rot, Vector3 cameraOffsetLocalSpace);

        [InternalMethod]
        private delegate Quaternion GetReferenceOrientationDelegate(CinemachineComponentBase component, BindingMode bindingMode, Vector3 worldUp);
    }
}
#endif