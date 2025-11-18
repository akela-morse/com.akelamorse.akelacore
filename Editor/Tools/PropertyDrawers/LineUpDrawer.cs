using System.Collections.Generic;
using System.Linq;
using Akela.Tools;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Tools
{
    [CustomPropertyDrawer(typeof(LineUpAttribute))]
    internal class LineUpDrawer : PropertyDrawer
    {
        private const float MARGIN = 4f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var prevWidth = EditorGUIUtility.labelWidth;
            var attr = (LineUpAttribute)attribute;

            var propRect = attr.noPrefix ? position : EditorGUI.PrefixLabel(position, label);

            EditorGUIUtility.labelWidth = propRect.width / 6f;

            using var unused = property.Copy();
            var count = GetDirectChildren(unused).Count();
            var index = 0;

            var previousRect = Rect.zero;

            using var copy = property.Copy();
            foreach (var prop in GetDirectChildren(copy))
            {
                var currentRect = new Rect(propRect);
                currentRect.x += (previousRect.width + MARGIN) * index;

                if (attr.sizeRatios != null && attr.sizeRatios.Length > index)
                    currentRect.width = (propRect.width - MARGIN * (count - 1)) * attr.sizeRatios[index];
                else
                    currentRect.width = (propRect.width - MARGIN * (count - 1)) / count;

                GUIContent propertyLabel = attr.Labels != null && attr.Labels.Length > index && !string.IsNullOrEmpty(attr.Labels[index]) ? new(attr.Labels[index]) : new();

                EditorGUI.PropertyField(currentRect, prop, propertyLabel);

                previousRect = currentRect;

                ++index;
            }

            EditorGUIUtility.labelWidth = prevWidth;
        }

        private static IEnumerable<SerializedProperty> GetDirectChildren(SerializedProperty parent)
        {
            var dots = parent.propertyPath.Count(c => c == '.');

            foreach (SerializedProperty inner in parent)
            {
                var isDirectChild = inner.propertyPath.Count(c => c == '.') == dots + 1;

                if (isDirectChild)
                    yield return inner;
            }
        }
    }
}