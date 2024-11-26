using Akela.Behaviours;
using UnityEngine;

namespace Akela.Motion
{
    [AddComponentMenu("Motion/Transform Lock", 0)]
    [HideScriptField, ExecuteInEditMode, DisallowMultipleComponent]
    public class TransformLock : MonoBehaviour
    {
        #region Component Fields
        [Header("Position")]
        [SerializeField] bool _lockPosX;
        [SerializeField] bool _lockPosY;
        [SerializeField] bool _lockPosZ;
        [SerializeField] Vector3 _lockedPosition;

        [Header("Rotation")]
        [SerializeField] bool _lockRotX;
        [SerializeField] bool _lockRotY;
        [SerializeField] bool _lockRotZ;
        [SerializeField] Vector3 _lockedRotation;

        [Header("Scale")]
        [SerializeField] bool _lockScaleX;
        [SerializeField] bool _lockScaleY;
        [SerializeField] bool _lockScaleZ;
        [SerializeField] Vector3 _lockedScale;

        [Space]
        [SerializeField] bool _onlyOnStart;
        #endregion

        #region Component Messages
        private void Start()
        {
            Lock();
        }

        private void Update()
        {
#if !UNITY_EDITOR
            if (_onlyOnStart)
                return;
#else
            if (_onlyOnStart && Application.isPlaying)
                return;
#endif

            Lock();
        }
        #endregion

        private void Lock()
        {
            // Position
            if (_lockPosX || _lockPosY || _lockPosZ)
            {
                var pos = transform.position;

                if (_lockPosX)
                    pos.x = _lockedPosition.x;

                if (_lockPosY)
                    pos.y = _lockedPosition.y;

                if (_lockPosZ)
                    pos.z = _lockedPosition.z;

                transform.position = pos;
            }

            // Rotation
            if (_lockRotX || _lockRotY || _lockRotZ)
            {
                var rot = transform.eulerAngles;

                if (_lockRotX)
                    rot.x = _lockedRotation.x;

                if (_lockRotY)
                    rot.y = _lockedRotation.y;

                if (_lockRotZ)
                    rot.z = _lockedRotation.z;

                transform.rotation = Quaternion.Euler(rot);
            }

            // Scale
            if (_lockScaleX || _lockScaleY || _lockScaleZ)
            {
                var scale = transform.localScale;

                if (_lockScaleX)
                    scale.x = _lockedScale.x;

                if (_lockScaleY)
                    scale.y = _lockedScale.y;

                if (_lockScaleZ)
                    scale.z = _lockedScale.z;

                transform.localScale = scale;
            }
        }
    }
}
