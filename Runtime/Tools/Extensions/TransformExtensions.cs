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
            foreach (Transform t in root)
            {
                yield return t;

                if (t.childCount > 0)
                {
                    foreach (var tChild in t.LoopRecursively())
                        yield return tChild;
                }
            }
        }

        public static Transform FindChildRecursive(this Transform root, Func<Transform, bool> predicate)
        {
            foreach (Transform t in root)
            {
                if (predicate(t))
                {
                    return t;
                }
                else if (t.childCount > 0)
                {
                    var child = t.FindChildRecursive(predicate);

                    if (child)
                        return child;
                }
            }

            return null;
        }

        public static Transform FindParentRecursive(this Transform leaf, Func<Transform, bool> predicate)
        {
            if (predicate(leaf))
                return leaf;
            else if (leaf.parent)
                return leaf.parent.FindParentRecursive(predicate);

            return null;
        }

        public static void SetLayerRecursively(this Transform trans, int layer)
        {
            trans.gameObject.layer = layer;
            foreach (Transform child in trans)
            {
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

            var min = new Vector3();
            min.x = Mathf.Min(xa.x, xb.x) + Mathf.Min(ya.x, yb.x) + Mathf.Min(za.x, zb.x) + col4Pos.x;
            min.y = Mathf.Min(xa.y, xb.y) + Mathf.Min(ya.y, yb.y) + Mathf.Min(za.y, zb.y) + col4Pos.y;
            min.z = Mathf.Min(xa.z, xb.z) + Mathf.Min(ya.z, yb.z) + Mathf.Min(za.z, zb.z) + col4Pos.z;

            var max = new Vector3();
            max.x = Mathf.Max(xa.x, xb.x) + Mathf.Max(ya.x, yb.x) + Mathf.Max(za.x, zb.x) + col4Pos.x;
            max.y = Mathf.Max(xa.y, xb.y) + Mathf.Max(ya.y, yb.y) + Mathf.Max(za.y, zb.y) + col4Pos.y;
            max.z = Mathf.Max(xa.z, xb.z) + Mathf.Max(ya.z, yb.z) + Mathf.Max(za.z, zb.z) + col4Pos.z;

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