#if AKELA_MATHEMATICS
using Unity.Mathematics;
using UnityEngine;

namespace Akela.Tools
{
    public static class MathematicsExtensions
    {
        public static Vector3Int ToVector3Int(this int3 v) => new(v.x, v.y, v.z);
        public static int3 ToInt3(this Vector3Int v) => new(v.x, v.y, v.z);
    }
}
#endif