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
        public static int3 ToInt3(this Vector3Int v) => new(v.x, v.y, v.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion ToQuaternion(this float4 q) => new(q.x, q.y, q.z, q.w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 ToFloat4(this Quaternion q) => new(q.x, q.y, q.z, q.w);
    }
}
#endif