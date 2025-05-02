using System.Runtime.CompilerServices;
using UnityEngine;

namespace Akela.Tools
{
    public static class VectorHelpers
    {
        public static readonly Vector2 null2 = new(float.NaN, float.NaN);
        public static readonly Vector3 null3 = new(float.NaN, float.NaN, float.NaN);
        public static readonly Vector4 null4 = new(float.NaN, float.NaN, float.NaN, float.NaN);

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
    }
}