using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            return GetClosestToDirection(transform, reference.forward);
        }

        public static Vector3 GuessUp(this Transform transform, Transform reference)
        {
            return GetClosestToDirection(transform, reference.up);
        }

        public static Vector3 GuessRight(this Transform transform, Transform reference)
        {
            return GetClosestToDirection(transform, reference.right);
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
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

            directions.Sort((a, b) => Vector3.Angle(b, direction).CompareTo(Vector3.Angle(a, direction)));

            return directions.First();
        }
    }
}