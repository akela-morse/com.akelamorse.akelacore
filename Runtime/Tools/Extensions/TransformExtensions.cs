using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Akela.Tools
{
    public static class TransformExtensions
    {
        public static IEnumerable<Transform> LoopRecursively(this Transform root)
        {
            for (var i = 0; i < root.childCount; ++i)
            {
                var t = root.GetChild(i);

                yield return t;

                foreach (var tChild in t.LoopRecursively())
                    yield return tChild;
            }
        }

		public static Transform FindChild(this Transform root, Func<Transform, bool> predicate)
        {
            for (var i = 0; i < root.childCount; ++i)
            {
                var t = root.GetChild(i);

                if (predicate(t))
                    return t;
            }

            return null;
        }

        public static Transform FindChildRecursive(this Transform root, Func<Transform, bool> predicate)
        {
            for (var i = 0; i < root.childCount; ++i)
            {
                var t = root.GetChild(i);

                if (predicate(t))
                    return t;

                var tChild = t.FindChildRecursive(predicate);

                if (tChild)
                    return tChild;
            }

            return null;
        }

        public static Transform FindParentRecursive(this Transform leaf, Func<Transform, bool> predicate)
        {
            for (;;)
            {
                if (predicate(leaf))
                    return leaf;

                if (leaf.parent)
                {
                    leaf = leaf.parent;
                    continue;
                }

                return null;
            }
        }

        public static void SetLayerRecursively(this Transform trans, int layer)
        {
            trans.gameObject.layer = layer;

            for (var i = 0; i < trans.childCount; ++i)
            {
                var child = trans.GetChild(i);

                child.SetLayerRecursively(layer);
            }
        }

        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }

        public static Bounds TransformBounds(this Transform transform, in Bounds bounds)
        {
            return MatrixTransformBounds(bounds, transform.worldToLocalMatrix);
        }

        public static Bounds InverseTransformBounds(this Transform transform, in Bounds bounds)
        {
            return MatrixTransformBounds(bounds, transform.localToWorldMatrix);
        }

        private static Bounds MatrixTransformBounds(in Bounds bounds, Matrix4x4 matrix)
        {
            var xa = matrix.GetColumn(0) * bounds.min.x;
            var xb = matrix.GetColumn(0) * bounds.max.x;

            var ya = matrix.GetColumn(1) * bounds.min.y;
            var yb = matrix.GetColumn(1) * bounds.max.y;

            var za = matrix.GetColumn(2) * bounds.min.z;
            var zb = matrix.GetColumn(2) * bounds.max.z;

            var col4Pos = matrix.GetColumn(3);

            var min = new Vector3
            {
                x = Mathf.Min(xa.x, xb.x) + Mathf.Min(ya.x, yb.x) + Mathf.Min(za.x, zb.x) + col4Pos.x,
                y = Mathf.Min(xa.y, xb.y) + Mathf.Min(ya.y, yb.y) + Mathf.Min(za.y, zb.y) + col4Pos.y,
                z = Mathf.Min(xa.z, xb.z) + Mathf.Min(ya.z, yb.z) + Mathf.Min(za.z, zb.z) + col4Pos.z
            };

            var max = new Vector3
            {
                x = Mathf.Max(xa.x, xb.x) + Mathf.Max(ya.x, yb.x) + Mathf.Max(za.x, zb.x) + col4Pos.x,
                y = Mathf.Max(xa.y, xb.y) + Mathf.Max(ya.y, yb.y) + Mathf.Max(za.y, zb.y) + col4Pos.y,
                z = Mathf.Max(xa.z, xb.z) + Mathf.Max(ya.z, yb.z) + Mathf.Max(za.z, zb.z) + col4Pos.z
            };

            return BoundsHelpers.CreateMinMax(min, max);
        }

        public static Vector3 GuessHeading(this Transform transform)
        {
            return GuessHeading(transform, transform.root);
        }

        public static Vector3 GuessUp(this Transform transform)
        {
            return GuessUp(transform, transform.root);
        }

        public static Vector3 GuessRight(this Transform transform)
        {
            return GuessRight(transform, transform.root);
        }

        public static Vector3 GuessHeading(this Transform transform, Transform reference)
        {
            return GetClosestToDirection(transform, reference == transform ? Vector3.forward : reference.forward);
        }

        public static Vector3 GuessUp(this Transform transform, Transform reference)
        {
            return GetClosestToDirection(transform, reference == transform ? Vector3.up : reference.up);
        }

        public static Vector3 GuessRight(this Transform transform, Transform reference)
        {
            return GetClosestToDirection(transform, reference == transform ? Vector3.right : reference.right);
        }

        private static Vector3 GetClosestToDirection(Transform transform, Vector3 direction)
        {
            var directions = new List<Vector3>
            {
                transform.right,
                transform.up,
                transform.forward,
                -transform.right,
                -transform.up,
                -transform.forward
            };

            directions.Sort((a, b) => Vector3.Angle(a, direction).CompareTo(Vector3.Angle(b, direction)));

            return directions.First();
        }
    }
}