using UnityEngine;

namespace Akela.Tools
{
    public static class DebugHelpers
    {
        public static void DrawQuad(Rect r, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Debug.DrawLine(new Vector3(r.x, 0f, r.y), new Vector3(r.xMax - 1, 0f, r.y), color, duration, depthTest);
            Debug.DrawLine(new Vector3(r.xMax - 1, 0f, r.y), new Vector3(r.xMax - 1, 0f, r.yMax - 1), color, duration, depthTest);
            Debug.DrawLine(new Vector3(r.xMax - 1, 0f, r.yMax - 1), new Vector3(r.x, 0f, r.yMax - 1), color, duration, depthTest);
            Debug.DrawLine(new Vector3(r.x, 0f, r.yMax - 1), new Vector3(r.x, 0f, r.y), color, duration, depthTest);
        }

        //Draws just the box at where it is currently hitting.
        public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color)
        {
            origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
            DrawBox(origin, halfExtents, orientation, color);
        }

        //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
        public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
        {
            direction.Normalize();
            Box bottomBox = new(origin, halfExtents, orientation);
            Box topBox = new(origin + (direction * distance), halfExtents, orientation);

            Debug.DrawLine(bottomBox.BackBottomLeft, topBox.BackBottomLeft, color);
            Debug.DrawLine(bottomBox.BackBottomRight, topBox.BackBottomRight, color);
            Debug.DrawLine(bottomBox.BackTopLeft, topBox.BackTopLeft, color);
            Debug.DrawLine(bottomBox.BackTopRight, topBox.BackTopRight, color);
            Debug.DrawLine(bottomBox.FrontTopLeft, topBox.FrontTopLeft, color);
            Debug.DrawLine(bottomBox.FrontTopRight, topBox.FrontTopRight, color);
            Debug.DrawLine(bottomBox.FrontBottomLeft, topBox.FrontBottomLeft, color);
            Debug.DrawLine(bottomBox.FrontBottomRight, topBox.FrontBottomRight, color);

            DrawBox(bottomBox, color);
            DrawBox(topBox, color);
        }

        public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
        {
            DrawBox(origin, halfExtents, orientation, color, 0f);
        }

        public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color, float duration)
        {
            DrawBox(new Box(origin, halfExtents, orientation), color, duration);
        }

        public static void DrawBox(Box box, Color color)
        {
            DrawBox(box, color, 0f);
        }

        public static void DrawBox(Box box, Color color, float duration)
        {
            Debug.DrawLine(box.FrontTopLeft, box.FrontTopRight, color, duration);
            Debug.DrawLine(box.FrontTopRight, box.FrontBottomRight, color, duration);
            Debug.DrawLine(box.FrontBottomRight, box.FrontBottomLeft, color, duration);
            Debug.DrawLine(box.FrontBottomLeft, box.FrontTopLeft, color, duration);

            Debug.DrawLine(box.BackTopLeft, box.BackTopRight, color, duration);
            Debug.DrawLine(box.BackTopRight, box.BackBottomRight, color, duration);
            Debug.DrawLine(box.BackBottomRight, box.BackBottomLeft, color, duration);
            Debug.DrawLine(box.BackBottomLeft, box.BackTopLeft, color, duration);

            Debug.DrawLine(box.FrontTopLeft, box.BackTopLeft, color, duration);
            Debug.DrawLine(box.FrontTopRight, box.BackTopRight, color, duration);
            Debug.DrawLine(box.FrontBottomRight, box.BackBottomRight, color, duration);
            Debug.DrawLine(box.FrontBottomLeft, box.BackBottomLeft, color, duration);
        }

        public struct Box
        {
            public Vector3 LocalFrontTopLeft { get; private set; }
            public Vector3 LocalFrontTopRight { get; private set; }
            public Vector3 LocalFrontBottomLeft { get; private set; }
            public Vector3 LocalFrontBottomRight { get; private set; }
            public readonly Vector3 LocalBackTopLeft => -LocalFrontBottomRight;
            public readonly Vector3 LocalBackTopRight => -LocalFrontBottomLeft;
            public readonly Vector3 LocalBackBottomLeft => -LocalFrontTopRight;
            public readonly Vector3 LocalBackBottomRight => -LocalFrontTopLeft;

            public readonly Vector3 FrontTopLeft => LocalFrontTopLeft + Origin;
            public readonly Vector3 FrontTopRight => LocalFrontTopRight + Origin;
            public readonly Vector3 FrontBottomLeft => LocalFrontBottomLeft + Origin;
            public readonly Vector3 FrontBottomRight => LocalFrontBottomRight + Origin;
            public readonly Vector3 BackTopLeft => LocalBackTopLeft + Origin;
            public readonly Vector3 BackTopRight => LocalBackTopRight + Origin;
            public readonly Vector3 BackBottomLeft => LocalBackBottomLeft + Origin;
            public readonly Vector3 BackBottomRight => LocalBackBottomRight + Origin;

            public Vector3 Origin { get; private set; }

            public Box(Vector3 origin, Vector3 halfExtents)
            {
                LocalFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
                LocalFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
                LocalFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
                LocalFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

                Origin = origin;
            }

            public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
            {
                Rotate(orientation);
            }

            public void Rotate(Quaternion orientation)
            {
                LocalFrontTopLeft = RotatePointAroundPivot(LocalFrontTopLeft, Vector3.zero, orientation);
                LocalFrontTopRight = RotatePointAroundPivot(LocalFrontTopRight, Vector3.zero, orientation);
                LocalFrontBottomLeft = RotatePointAroundPivot(LocalFrontBottomLeft, Vector3.zero, orientation);
                LocalFrontBottomRight = RotatePointAroundPivot(LocalFrontBottomRight, Vector3.zero, orientation);
            }

            public static Box CreateFromMinMax(Vector3 min, Vector3 max)
            {
                var extents = (max - min) * 0.5f;
                var center = min + extents;

                return new Box(center, extents);
            }
        }

        //This should work for all cast types
        private static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
        {
            return origin + (direction.normalized * hitInfoDistance);
        }

        private static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            var direction = point - pivot;
            return pivot + rotation * direction;
        }
    }
}