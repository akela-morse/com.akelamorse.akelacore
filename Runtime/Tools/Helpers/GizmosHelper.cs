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
            DrawCapsuleMethod(bottom, top, radius, true);
        }

        public static void DrawWireCapsule(Vector3 center, Vector3 axis, float radius, float height)
        {
            var offset = (height - radius * 2f) * .5f;

            if (offset < 0f)
            {
                DrawCapsuleMethod(center, center, radius, false);
                return;
            }

            var top = center + axis * offset;
            var bottom = center - axis * offset;

            DrawCapsuleMethod(bottom, top, radius, true);
        }

        public static void DrawCapsule(Vector3 bottom, Vector3 top, float radius)
        {
            DrawCapsuleMethod(bottom, top, radius, false);
        }

        public static void DrawCapsule(Vector3 center, Vector3 axis, float radius, float height)
        {
            var offset = (height - radius * 2f) * .5f;

            if (offset < 0f)
            {
                DrawCapsuleMethod(center, center, radius, false);
                return;
            }

            var top = center + axis * offset;
            var bottom = center - axis * offset;

            DrawCapsuleMethod(bottom, top, radius, false);
        }

        private static void DrawCapsuleMethod(Vector3 bottom, Vector3 top, float radius, bool wire)
        {
            if (bottom == top)
            {
                if (wire)
                    Gizmos.DrawWireSphere(bottom, radius);
                else
                    Gizmos.DrawMesh(Resources.GetBuiltinResource<Mesh>("Sphere.fbx"), bottom, Quaternion.identity, Vector3.one * radius);

                return;
            }

            var mainAxis = (top - bottom).normalized;

            if (wire)
            {
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
            else
            {
                var center = (top + bottom) * .5f;
                var rotation = QuaternionHelpers.OmniLookRotation(Vector3.up, mainAxis, Vector3.forward);
                var scale = new Vector3(radius, (top - bottom).magnitude * .5f, radius);

                Gizmos.DrawMesh(Resources.GetBuiltinResource<Mesh>("Sphere.fbx"), top, rotation, Vector3.one * radius);
                Gizmos.DrawMesh(Resources.GetBuiltinResource<Mesh>("Sphere.fbx"), bottom, rotation, Vector3.one * radius);
                Gizmos.DrawMesh(Resources.GetBuiltinResource<Mesh>("Cylinder.fbx"), center, rotation, scale);
            }
        }

        public static void DrawViewportSphere(Vector3 position, float radius)
        {
            using (new Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
            {
                Handles.DrawWireDisc(position, Vector3.right, radius);
                Handles.DrawWireDisc(position, Vector3.up, radius);
                Handles.DrawWireDisc(position, Vector3.forward, radius);

                if (Camera.current.orthographic)
                {
                    var normal = position - Handles.inverseMatrix.MultiplyVector(Camera.current.transform.forward);
                    var sqrMagnitude = normal.sqrMagnitude;
                    var num0 = radius * radius;

                    Handles.DrawWireDisc(position - num0 * normal / sqrMagnitude, normal, radius);
                }
                else
                {
                    var normal = position - Handles.inverseMatrix.MultiplyPoint(Camera.current.transform.position);
                    var sqrMagnitude = normal.sqrMagnitude;
                    var num0 = radius * radius;
                    var num1 = num0 * num0 / sqrMagnitude;
                    var num2 = Mathf.Sqrt(num0 - num1);

                    Handles.DrawWireDisc(position - num0 * normal / sqrMagnitude, normal, num2);
                }
            }
        }

        public static void DrawCone(Vector3 peak, Vector3 axis, float angle, float minRange, float maxRange)
        {
            var tan = Mathf.Tan(angle);

            using (new Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
            {
                Handles.DrawWireDisc(peak + axis * minRange, axis, minRange * tan);
                Handles.DrawWireDisc(peak + axis * maxRange, axis, maxRange * tan);
            }
        }
    }
}
#endif