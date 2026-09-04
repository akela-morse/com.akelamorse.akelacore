using System.Runtime.CompilerServices;
using UnityEngine;

namespace Akela.Tools
{
    public static class QuaternionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion ExpandByFactor(this Quaternion quaternion, float factor)
        {
            quaternion.ToAngleAxis(out var angle, out var axis);
            return Quaternion.AngleAxis(angle * factor, axis);
        }
    }
}