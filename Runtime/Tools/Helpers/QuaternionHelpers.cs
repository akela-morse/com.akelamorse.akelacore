using UnityEngine;

namespace Akela.Tools
{
    public static class QuaternionHelpers
    {
        public static Quaternion OmniLookRotation(Vector3 aimAxis, Vector3 direction, Vector3 upAxis)
        {
            return OmniLookRotation(aimAxis, direction, upAxis, Vector3.up);
        }

        public static Quaternion OmniLookRotation(Vector3 aimAxis, Vector3 direction, Vector3 upAxis, Vector3 worldUp)
        {
            var zyToCustom = Quaternion.LookRotation(aimAxis, upAxis);
            var customToZY = Quaternion.Inverse(zyToCustom);

            var zyToTarget = Quaternion.LookRotation(direction, worldUp);

            var customToTarget = zyToTarget * customToZY;

            return customToTarget;
        }

        public static Vector3 AngularVelocity(Quaternion previousRotation, Quaternion currentRotation)
        {
            const float epsilon = 1023.5f / 1024.0f;

            var q = currentRotation * Quaternion.Inverse(previousRotation);

            var absQ = Mathf.Abs(q.w);

            if (absQ > epsilon)
                return Vector3.zero;

            var angle = Mathf.Acos(absQ);
            var gain = Mathf.Sign(q.w) * 2f * angle / (Mathf.Sin(angle) * Time.deltaTime);

            return new Vector3(q.x * gain, q.y * gain, q.z * gain);
        }
    }
}