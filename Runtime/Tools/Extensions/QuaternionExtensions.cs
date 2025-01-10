using System.Runtime.CompilerServices;
using UnityEngine;

namespace Akela.Tools
{
    public static class QuaternionExtensions
    {
        /// <summary>
        /// Checks if the rotation expressed by this quaternion is void, regardless of whether or not the quaternion is normalised
        /// </summary>
        /// <returns>True if the rotation is void, False otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsVoid(this Quaternion quaternion)
        {
            return Mathf.Approximately(quaternion.x, 0f) && Mathf.Approximately(quaternion.y, 0f) && Mathf.Approximately(quaternion.z, 0f);
        }
    }
}