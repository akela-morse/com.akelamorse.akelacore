#if AKELA_MATHEMATICS
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Akela.Tools
{
    public static class MathematicsExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ToVector3Int(this int3 v) => new(v.x, v.y, v.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int3 ToInt3(this Vector3Int v) => math.int3(v.x, v.y, v.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion ToUnityQuaternion(this quaternion q) => new(q.value.x, q.value.y, q.value.z, q.value.w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion ToMathQuaternion(this Quaternion q) => math.quaternion(q.x, q.y, q.z, q.w);
    }
}
#endif