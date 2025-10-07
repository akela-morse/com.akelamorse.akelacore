using UnityEngine;

namespace Akela.Tools
{
    public enum Axis
    {
        [InspectorName("X-Axis")] X,
        [InspectorName("Y-Axis")] Y,
        [InspectorName("Z-Axis")] Z
    }

    public static class AxisExtensions
    {
        public static Vector3 ToVector3(this Axis axis) => axis switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => Vector3.zero
        };

        public static Vector2 ToVector2(this Axis axis) => axis switch
        {
            Axis.X => Vector2.right,
            Axis.Y => Vector2.up,
            _ => Vector2.zero
        };

        public static Vector3 UpRelative(this Axis axis) => axis switch
        {
            Axis.X => Vector3.up,
            Axis.Y => Vector3.back,
            Axis.Z => Vector3.up,
            _ => Vector3.zero,
        };

        public static Vector3 RightRelative(this Axis axis) => axis switch
        {
            Axis.X => Vector3.back,
            Axis.Y or Axis.Z => Vector3.right,
            _ => Vector3.zero,
        };
    }
}