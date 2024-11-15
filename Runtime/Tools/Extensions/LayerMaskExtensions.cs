using UnityEngine;

namespace Akela.Tools
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static LayerMask Add(this LayerMask mask, int layer)
        {
            return mask | (1 << layer);
        }
    }
}