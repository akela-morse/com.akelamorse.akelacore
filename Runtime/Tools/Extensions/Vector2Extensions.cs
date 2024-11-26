using UnityEngine;

namespace Akela.Tools
{
    public static class Vector2Extensions
    {
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = cos * tx - sin * ty;
            v.y = sin * tx + cos * ty;

            return v;
        }

        public static Vector2Int Rotate(this Vector2Int v, float degrees)
        {
            return Vector2Int.RoundToInt(((Vector2)v).Rotate(degrees).normalized);
        }

        public static Vector3 ToTopviewVector3(this Vector2 v)
        {
            return new Vector3(v.x, 0f, v.y);
        }

        public static Vector3 ToTopviewVector3(this Vector2Int v)
        {
            return new Vector3(v.x, 0f, v.y);
        }

        public static Vector2 Round(this Vector2 v)
        {
            return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        }
    }
}