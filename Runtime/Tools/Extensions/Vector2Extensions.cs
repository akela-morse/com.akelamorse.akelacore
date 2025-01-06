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
    }
}