using UnityEngine;

namespace Akela.Tools
{
	public static class VectorHelpers
	{
		public static float GreatCircleDistance(Vector3 from, Vector3 to, float radius = 1f)
		{
			return Mathf.Acos(Vector3.Dot(from.normalized, to.normalized)) * radius;
		}

		public static float GreatCircleDistance(Vector3 from, Vector3 to, Vector3 center, float radius = 1f)
		{
			return GreatCircleDistance(from - center, to - center, radius);
		}

		public static Vector3 ScaleInverse(Vector3 a, Vector3 b)
		{
			return Vector3.Scale(a, b.Inverse());
		}
	}
}