using UnityEngine;

namespace Akela.Tools
{
    public static class RectExtensions
    {
        public static Rect Scale(this Rect r, float factor)
        {
            r.min *= factor;
            r.max *= factor;

            return r;
        }
    }
}