#if AKELA_CINEMACHINE
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Akela.Cinemachine
{
    [SaveDuringPlay]
    // [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/TorusCollider Icon.png")]
    [AddComponentMenu("Cinemachine/Helpers/Cinemachine Impulse To Motors")]
    public class CinemachineImpulseToMotors : MonoBehaviour
    {
        public static bool Enabled { get; set; } = true;

        [Tooltip("Impulse events on channels not included in the mask will be ignored.")]
        [CinemachineImpulseChannelProperty]
        [SerializeField] private int _channelMask = 1;

        [Tooltip("Gain to apply to the Impulse signal.  1 is normal strength.  Setting this to 0 completely mutes the signal.")]
        [SerializeField] private  float _gain = 1f;

        [Tooltip("Enable this to perform distance calculation in 2D (ignore Z).")]
        [SerializeField] private  bool _use2DDistance;

        [Tooltip("Enable this to process all impulse signals in local space.")]
        [SerializeField] private  bool _useLocalSpace = true;

        [Tooltip("This controls the secondary reaction of the listener to the incoming impulse.  "
            + "The impulse might be for example a sharp shock, and the secondary reaction could "
            + "be a vibration whose amplitude and duration is controlled by the size of the "
            + "original impulse.  This allows different listeners to respond in different ways "
            + "to the same impulse signal.")]
        [SerializeField] private  CinemachineImpulseListener.ImpulseReaction _reactionSettings = new()
        {
            AmplitudeGain = 1,
            FrequencyGain = 1,
            Duration = 1f
        };

        private Camera _camera;

        #region Component Fields
        private void OnEnable()
        {
            _camera = Camera.main;

            InputSystem.onAfterUpdate += InputUpdate;
        }

        private void OnDisable()
        {
            InputSystem.onAfterUpdate -= InputUpdate;
        }
        #endregion

        private void InputUpdate()
        {
            if (!Enabled || Gamepad.current == null)
                return;

            var haveImpulse = CinemachineImpulseManager.Instance.GetImpulseAt(transform.position, _use2DDistance, _channelMask, out var impulsePos, out _);
            var haveReaction = _reactionSettings.GetReaction(Time.deltaTime, impulsePos, out var reactionPos, out _);

            if (haveReaction)
                impulsePos += reactionPos;

            if (haveImpulse || haveReaction)
            {
                if (_useLocalSpace)
                    impulsePos = transform.rotation * impulsePos;

                var parameterisedImpulse =  new Vector2(
                    Vector3.Dot(_camera.transform.right, impulsePos),
                    Vector3.Dot(_camera.transform.forward, impulsePos)
                );

                var theta = Vector2.Angle(parameterisedImpulse, Vector2.right);
                var absSin = Mathf.Abs(Mathf.Sin(theta));
                var cos = Mathf.Cos(theta);

                Gamepad.current.SetMotorSpeeds((absSin - cos) * _gain, (absSin + cos) * _gain);
            }
            else
            {
                Gamepad.current.ResetHaptics();
            }
        }
    }
}
#endif