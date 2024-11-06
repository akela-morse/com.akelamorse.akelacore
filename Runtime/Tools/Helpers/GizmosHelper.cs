#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Akela.Tools
{
	public static class GizmosHelper
	{
		public static void DrawThickLine(Vector3 from, Vector3 to, float thickness)
		{
			using (new Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
			{				
				Handles.DrawLine(from, to, thickness);
			}
		}

		public static void DrawDottedLine(Vector3 from, Vector3 to, float size)
		{
			using (new Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
			{
				Handles.DrawDottedLine(from, to, size);
			}
		}

		public static void DrawArrow(Vector3 origin, Vector3 direction, float size)
		{
			var orientation = Quaternion.LookRotation(direction);

			using (new Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
			{
				Handles.ArrowHandleCap(0, origin, orientation, size, EventType.Repaint);
			}
		}

		public static void DrawWireCapsule(Vector3 bottom, Vector3 top, float radius)
		{
			if (bottom == top)
			{
				Gizmos.DrawWireSphere(bottom, radius);
				return;
			}

			var mainAxis = (top - bottom).normalized;

			var rightAxis = Vector3.Cross(mainAxis, Vector3.up).normalized;

			if (rightAxis.sqrMagnitude == 0f)
				rightAxis = Vector3.right;

			var upAxis = Vector3.Cross(mainAxis, rightAxis).normalized;

			using (new Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
			{
				Handles.DrawWireDisc(top, mainAxis, radius);
				Handles.DrawWireDisc(bottom, mainAxis, radius);

				Handles.DrawLine(top + rightAxis * radius, bottom + rightAxis * radius);
				Handles.DrawLine(top - rightAxis * radius, bottom - rightAxis * radius);
				Handles.DrawLine(top + upAxis * radius, bottom + upAxis * radius);
				Handles.DrawLine(top - upAxis * radius, bottom - upAxis * radius);

				Handles.DrawWireArc(top, upAxis, rightAxis, -180f, radius);
				Handles.DrawWireArc(top, rightAxis, upAxis, 180f, radius);
				Handles.DrawWireArc(bottom, upAxis, rightAxis, 180f, radius);
				Handles.DrawWireArc(bottom, rightAxis, upAxis, -180f, radius);
			}
		}

		public static void DrawWireCapsule(Vector3 center, Vector3 axis, float radius, float height)
		{
			var offset = (height - (radius * 2f)) * .5f;
			var top = center + axis * offset;
			var bottom = center - axis * offset;

			DrawWireCapsule(bottom, top, radius);
		}
	}
}
#endif