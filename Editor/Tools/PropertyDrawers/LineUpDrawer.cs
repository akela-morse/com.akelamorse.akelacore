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

            foreach (SerializedProperty prop in property.Copy())
                count++;

            var propRect = EditorGUI.PrefixLabel(position, label);
            propRect.width = (propRect.width - MARGIN * (count - 1)) / count;

            EditorGUIUtility.labelWidth = propRect.width / 4f;

            foreach (SerializedProperty prop in property.Copy())
            {
                var currentRect = new Rect(propRect);
                currentRect.x += (currentRect.width + MARGIN) * index;

                GUIContent propertyLabel = attr.Labels != null && attr.Labels.Length > index ? new(attr.Labels[index]) : new();

                EditorGUI.PropertyField(currentRect, prop, propertyLabel);

                ++index;
            }

            EditorGUIUtility.labelWidth = prevWidth;
        }
    }
}
