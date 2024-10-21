using UnityEngine;

namespace Akela.Tools
{
	public static class ColliderExtensions
	{
		public static Rect ToViewportRect(this BoxCollider box)
		{
			var camera = Camera.main;

			var cen = box.center;
			var ext = box.size;

			var extentPoints = new[]
			{
				camera.WorldToViewportPoint(box.transform.TransformPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z))),
				camera.WorldToViewportPoint(box.transform.TransformPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z))),
				camera.WorldToViewportPoint(box.transform.TransformPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z))),
				camera.WorldToViewportPoint(box.transform.TransformPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z))),
				camera.WorldToViewportPoint(box.transform.TransformPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z))),
				camera.WorldToViewportPoint(box.transform.TransformPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z))),
				camera.WorldToViewportPoint(box.transform.TransformPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z))),
				camera.WorldToViewportPoint(box.transform.TransformPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z)))
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
	}
}
