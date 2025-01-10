using System;
using Akela.Tools;
using UnityEngine;

namespace Akela.Motion
{
    [CreateAssetMenu(fileName = "New Transform Animation", menuName = "Animation/Transform Animation", order = 10)]
    public class TransformAnimation : ScriptableObject
    {
        [Serializable]
        public struct TransformAnimationKey
        {
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
            public float duration;
            public AnimationCurve fromToCurve;
        }

        #region Component Fields
        public TransformAnimationKey[] keys;
        #endregion

        public bool IsValid => keys.Length > 0;

        public float Duration()
        {
            var totalDuration = 0f;

            foreach (var key in keys)
                totalDuration += key.duration;

            return totalDuration;
        }

        /// <summary>
        /// Evaluate the transform animation at the given time
        /// </summary>
        /// <param name="time">Time to evaluate the animation at</param>
        /// <param name="position">Resulting position</param>
        /// <param name="eulerRotation">Resulting rotation</param>
        /// <param name="scale">Resulting scale</param>
        /// <returns>True if the animation has not yet ended, False otherwise</returns>
        public bool Evaluate(float time, out Vector3 position, out Vector3 eulerRotation, out Vector3 scale)
        {
            position = Vector3.zero;
            eulerRotation = Vector3.zero;
            scale = Vector3.one;

            if (time < 0f)
                return false;

            var additionalTime = 0f;

            foreach (var key in keys)
            {
                additionalTime += key.duration;

                if (time > additionalTime)
                {
                    position = key.position;
                    eulerRotation = key.rotation;
                    scale = key.scale;

                    continue;
                }

                var delta = additionalTime - time;
                var percent = 1f - delta / key.duration;
                var value = key.fromToCurve.Evaluate(percent);

                position = Vector3.Lerp(position, key.position, value);
                eulerRotation = Vector3.Lerp(eulerRotation, key.rotation, value);
                scale = Vector3.Lerp(scale, key.scale, value);

                return true;
            }

            return false;
        }

        public void GetFirstKey(out Vector3 position, out Vector3 eulerRotation, out Vector3 scale)
        {
            position = keys[0].position;
            eulerRotation = keys[0].rotation;
            scale = keys[0].scale;
        }

        public void GetLastKey(out Vector3 position, out Vector3 eulerRotation, out Vector3 scale)
        {
            position = keys[^1].position;
            eulerRotation = keys[^1].rotation;
            scale = keys[^1].scale;
        }
    }
}