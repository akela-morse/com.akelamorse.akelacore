using UnityEngine;

namespace Akela.Tools
{
    public static class RendererExtensions
    {
        public static Renderer GetLargestRendererInLOD(this LODGroup lodGroup)
        {
            var firstLod = lodGroup.GetLODs()[0];
            var maxBoundDiag = float.MinValue;

            Renderer currentRender = null;

            foreach (var renderer in firstLod.renderers)
            {
                var cen = renderer.bounds.center;
                var ext = renderer.bounds.extents;

                var extMin = cen - ext;
                var extMax = cen + ext;

                var diag = Vector3.Distance(extMin, extMax);

                if (diag > maxBoundDiag)
                {
                    currentRender = renderer;
                    maxBoundDiag = diag;
                }
            }

            return currentRender;
        }
    }
}
