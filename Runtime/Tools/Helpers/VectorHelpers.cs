using System.Runtime.CompilerServices;
using UnityEngine;

namespace Akela.Tools
{
    public static class VectorHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }

        public static float GreatCircleDistance(Vector3 from, Vector3 to, float radius = 1f)
        {
            return Mathf.Acos(Vector3.Dot(from.normalized, to.normalized)) * radius;
        }

        public static float GreatCircleDistance(Vector3 from, Vector3 to, Vector3 center, float radius = 1f)
        {
            return GreatCircleDistance(from - center, to - center, radius);
        }

        public static Vector3 ScaleInverse(Vector3 a, Vector3 b)
        {
            return Vector3.Scale(a, b.Inverse());
        }

        public static Vector2 Rotate2D(Vector2 v, float degrees)
        {
            var sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            var cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            var tx = v.x;
            var ty = v.y;
            v.x = cos * tx - sin * ty;
            v.y = sin * tx + cos * ty;

            return v;
        }

        public static Vector2Int Rotate2D(Vector2Int v, float degrees)
        {
            return Vector2Int.RoundToInt(Rotate2D((Vector2)v, degrees));
        }

        public static Vector2 SmoothStep(Vector2 from, Vector2 to, float t)
        {
            var dt = (double)Mathf.Clamp01(t);
            dt = -2.0 * dt * dt * dt + 3.0 * dt * dt;
            return to * (float)dt + from * (1f - (float)dt);
        }

        public static Vector3 SmoothStep(Vector3 from, Vector3 to, float t)
        {
            var dt = (double)Mathf.Clamp01(t);
            dt = -2.0 * dt * dt * dt + 3.0 * dt * dt;
            return to * (float)dt + from * (1f - (float)dt);
        }

        public static Vector4 SmoothStep(Vector4 from, Vector4 to, float t)
        {
            var dt = (double)Mathf.Clamp01(t);
            dt = -2.0 * dt * dt * dt + 3.0 * dt * dt;
            return to * (float)dt + from * (1f - (float)dt);
        }
    }
}