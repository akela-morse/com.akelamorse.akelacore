using UnityEngine;

namespace Akela.Tools
{
    public static class BoundsExtensions
    {
        public static Rect ToViewportRect(this Bounds bounds)
        {
            var camera = Camera.main!;

            var cen = bounds.center;
            var ext = bounds.extents;

            var extentPoints = new[]
            {
                camera.WorldToViewportPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
                camera.WorldToViewportPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
                camera.WorldToViewportPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
                camera.WorldToViewportPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
                camera.WorldToViewportPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
                camera.WorldToViewportPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
                camera.WorldToViewportPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
                camera.WorldToViewportPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
            };

            var min = extentPoints[0];
            var max = extentPoints[0];

            foreach (var v in extentPoints)
            {
                min = Vector2.Min(min, v);
                max = Vector2.Max(max, v);
            }

            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }

        public static void NormalizeToOrigin(this ref Bounds bounds)
        {
            var newCenter = (bounds.max - bounds.min) / 2f;
            bounds.center = newCenter;
        }
    }
}