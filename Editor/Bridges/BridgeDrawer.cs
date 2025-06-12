using Akela.Bridges;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AkelaEditor.Bridges
{
    [CustomPropertyDrawer(typeof(IBridge), true)]
    internal class BridgeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_internalValue"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var actualProperty = property.FindPropertyRelative("_internalValue");

            EditorGUI.PropertyField(position, actualProperty, label);
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var actualProperty = property.FindPropertyRelative("_internalValue");

            return new PropertyField(actualProperty, preferredLabel);
        }
    }
}