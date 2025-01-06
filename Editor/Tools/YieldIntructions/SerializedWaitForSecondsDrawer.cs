using Akela.Tools;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.YieldInstructions
{
    [CustomPropertyDrawer(typeof(SerializedWaitForSeconds), true)]
    internal class SerializedWaitForSecondsDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_duration"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var actualProperty = property.FindPropertyRelative("_duration");

            EditorGUI.PropertyField(position, actualProperty, label);
        }
    }
}