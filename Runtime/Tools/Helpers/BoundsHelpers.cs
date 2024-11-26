using UnityEngine;

namespace Akela.Tools
{
    public static class BoundsHelpers
    {
        public static Bounds CreateMinMax(Vector3 min, Vector3 max)
        {
            var result = new Bounds();
            result.SetMinMax(min, max);

            return result;
        }
    }
}