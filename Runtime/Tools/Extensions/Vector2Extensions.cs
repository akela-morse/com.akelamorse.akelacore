using System.Runtime.CompilerServices;
using UnityEngine;

namespace Akela.Tools
{
    public static class Vector2Extensions
    {
        public static Vector3 ToTopviewVector3(this Vector2 v)
        {
            return new Vector3(v.x, 0f, v.y);
        }

        public static Vector3Int ToTopviewVector3(this Vector2Int v)
        {
            return new Vector3Int(v.x, 0, v.y);
        }

        public static Vector2 SnapToCardinalDirection(this Vector2 v)
        {
            if (v == Vector2.zero)
                return Vector2.zero;

            var absV = new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));

            if (absV.x > absV.y)
                return v.x > 0f ? Vector2.right : Vector2.left;

            return v.y > 0f ? Vector2.up : Vector2.down;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NormalizeWithMagnitude(this ref Vector2 v, float magnitude)
        {
            if (magnitude > 9.999999747378752E-06)
                v /= magnitude;
            else
                v = Vector2.zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetNormalizedAndMagnitude(this Vector2 v, out Vector2 normalized, out float magnitude)
        {
            magnitude = v.magnitude;

            if (magnitude > 9.999999747378752E-06)
                normalized = v / magnitude;
            else
                normalized = Vector2.zero;
        }
    }
}