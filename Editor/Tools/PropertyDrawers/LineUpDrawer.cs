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

            var count = 0;
            var index = 0;

            foreach (SerializedProperty unused in property.Copy())
                count++;

            var propRect = EditorGUI.PrefixLabel(position, label);

            EditorGUIUtility.labelWidth = propRect.width / 6f;

            var previousRect = Rect.zero;
            foreach (SerializedProperty prop in property.Copy())
            {
                var currentRect = new Rect(propRect);
                currentRect.x += (previousRect.width + MARGIN) * index;

                if (attr.sizeRatios != null && attr.sizeRatios.Length > index)
                    currentRect.width = (propRect.width - MARGIN * (count - 1)) * attr.sizeRatios[index];
                else
                    currentRect.width = (propRect.width - MARGIN * (count - 1)) / count;

                GUIContent propertyLabel = attr.Labels != null && attr.Labels.Length > index ? new(attr.Labels[index]) : new();

                EditorGUI.PropertyField(currentRect, prop, propertyLabel);

                previousRect = currentRect;

                ++index;
            }

            EditorGUIUtility.labelWidth = prevWidth;
        }
    }
}