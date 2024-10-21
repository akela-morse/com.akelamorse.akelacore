using System;
using System.Linq;
using UnityEngine;

namespace Akela.Tools
{
	public static class LODGroupExtensions
	{
		//Return the LODGroup component with a renderer pointing to a specific GameObject. If the GameObject is not part of a LODGroup, returns null 
		static public LODGroup GetParentLODGroup(this Renderer renderer)
		{
			var LODGroupParent = renderer.GetComponentInParent<LODGroup>();

			if (LODGroupParent == null)
				return null;

			var LODs = LODGroupParent.GetLODs();

			var foundLOD = LODs.Where(lod => lod.renderers.Where(x => renderer == x).Any()).Any();

			if (foundLOD)
				return LODGroupParent;

			return null;
		}

		//Get the LOD # of a selected GameObject. If the GameObject is not part of any LODGroup returns -1.
		static public int GetLODid(this Renderer renderer)
		{
			var LODGroupParent = renderer.GetComponentInParent<LODGroup>();

			if (LODGroupParent == null)
				return -1;

			var LODs = LODGroupParent.GetLODs();

			var index = Array.FindIndex(LODs, lod => lod.renderers.Where(x => renderer == x).Any());

			return index;
		}


		//returns the currently visible LOD level of a specific LODGroup, from a specific camera. If no camera is define, uses the Camera.current.
		public static int GetVisibleLOD(this LODGroup lodGroup, Camera camera = null)
		{
			var lods = lodGroup.GetLODs();
			var relativeHeight = GetRelativeHeight(lodGroup, camera == null ? Camera.current : camera);

			var lodIndex = lodGroup.lodCount - 1;

			for (var i = 0; i < lods.Length; i++)
			{
				var lod = lods[i];

				if (relativeHeight >= lod.screenRelativeTransitionHeight)
				{
					lodIndex = i;
					break;
				}
			}

			return lodIndex;
		}

		public static float GetWorldSpaceSize(this LODGroup lodGroup)
		{
			return GetWorldSpaceScale(lodGroup.transform) * lodGroup.size;
		}

		#region Private Methods
		static float GetRelativeHeight(LODGroup lodGroup, Camera camera)
		{
			var distance = (lodGroup.transform.TransformPoint(lodGroup.localReferencePoint) - camera.transform.position).magnitude;

			return DistanceToRelativeHeight(camera, (distance / QualitySettings.lodBias), GetWorldSpaceSize(lodGroup));
		}

		static float DistanceToRelativeHeight(Camera camera, float distance, float size)
		{
			if (camera.orthographic)
				return size * 0.5F / camera.orthographicSize;

			var halfAngle = Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView * 0.5F);
			var relativeHeight = size * 0.5F / (distance * halfAngle);

			return relativeHeight;
		}

		static float GetWorldSpaceScale(Transform t)
		{
			var scale = t.lossyScale;
			var largestAxis = Mathf.Abs(scale.x);

			largestAxis = Mathf.Max(largestAxis, Mathf.Abs(scale.y));
			largestAxis = Mathf.Max(largestAxis, Mathf.Abs(scale.z));

			return largestAxis;
		}
		#endregion
	}
}
