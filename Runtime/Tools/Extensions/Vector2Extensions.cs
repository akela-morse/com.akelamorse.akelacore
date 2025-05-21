using UnityEngine;

namespace Akela.Tools
{
    public static class Vector2Extensions
    {
        public static Vector3 ToTopviewVector3(this Vector2 v)
        {
            return new Vector3(v.x, 0f, v.y);
        }

        public static Vector3 ToTopviewVector3(this Vector2Int v)
        {
            return new Vector3(v.x, 0f, v.y);
        }

        public static Vector2 SnapToCardinalDirection(this Vector2 v)
        {
            if (v == Vector2.zero)
                return Vector2.zero;

            var absV = new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));

            if (absV.x > absV.y)
                return v.x > 0f ? Vector2.right : Vector2.left;
            else
                return v.y > 0f ? Vector2.up : Vector2.down;
        }
    }
}