using System;
using Akela.Tools;
using UnityEngine;

namespace Akela.Motion
{
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/TransformAnimationAsset Icon.png")]
    [CreateAssetMenu(fileName = "New Transform Animation", menuName = "Animation/Transform Animation", order = 10)]
    public class TransformAnimation : ScriptableObject
    {
        [Serializable]
        public struct TransformAnimationKey
        {
            public Vector3 position;
            [EulerAngles] public Quaternion rotation;
            public Vector3 scale;
            public float duration;
            public AnimationCurve fromToCurve;
        }

        #region Component Fields
        public TransformAnimationKey[] keys;
        #endregion

        public bool IsValid => keys is { Length: > 0 };

        public float Duration()
        {
            var totalDuration = 0f;

            for (var i = 0; i < keys.Length; i++)
                totalDuration += keys[i].duration;

            return totalDuration;
        }

        /// <summary>
        /// Evaluate the transform animation at the given time
        /// </summary>
        /// <param name="time">Time to evaluate the animation at</param>
        /// <param name="position">Resulting position</param>
        /// <param name="rotation">Resulting rotation</param>
        /// <param name="scale">Resulting scale</param>
        /// <returns>True if the animation has not yet ended, False otherwise</returns>
        public bool Evaluate(float time, out Vector3 position, out Quaternion rotation, out Vector3 scale)
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one;

            if (time < 0f)
                return false;

            var additionalTime = 0f;

            for (var i = 0; i < keys.Length; i++)
            {
                additionalTime += keys[i].duration;

                if (time > additionalTime)
                {
                    position = keys[i].position;
                    rotation = keys[i].rotation;
                    scale = keys[i].scale;

                    continue;
                }

                var delta = additionalTime - time;
                var percent = keys[i].duration == 0f ? 1f : 1f - delta / keys[i].duration;
                var value = keys[i].fromToCurve.Evaluate(percent);

                position = Vector3.Lerp(position, keys[i].position, value);
                rotation = Quaternion.Lerp(rotation, keys[i].rotation, value);
                scale = Vector3.Lerp(scale, keys[i].scale, value);

                return true;
            }

            return false;
        }

        public void GetFirstKey(out Vector3 position, out Quaternion rotation, out Vector3 scale)
        {
            position = keys[0].position;
            rotation = keys[0].rotation;
            scale = keys[0].scale;
        }

        public void GetLastKey(out Vector3 position, out Quaternion rotation, out Vector3 scale)
        {
            position = keys[^1].position;
            rotation = keys[^1].rotation;
            scale = keys[^1].scale;
        }
    }
}