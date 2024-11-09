using UnityEngine;

namespace Akela.Tools
{
	public static class Vector4Extensions
	{
		public static Vector4Int RoundToInt(this Vector4 v)
		{
			return new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z), Mathf.RoundToInt(v.w));
		}

		public static Vector4Int FloorToInt(this Vector4 v)
		{
			return new(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z), Mathf.FloorToInt(v.w));
		}

		public static Vector4Int CeilToInt(this Vector4 v)
		{
			return new(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y), Mathf.CeilToInt(v.z), Mathf.CeilToInt(v.w));
		}
	}
}