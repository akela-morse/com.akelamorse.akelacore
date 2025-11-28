using Akela.Behaviours;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor
{
    [CustomPropertyDrawer(typeof(FromThisAttribute))]
    [CustomPropertyDrawer(typeof(FromParentsAttribute))]
    [CustomPropertyDrawer(typeof(FromChildrenAttribute))]
    public class DependencyFromDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.DrawRect(new Rect(position) { x = 0f, width = 2f }, Color.darkOrchid);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndDisabledGroup();
        }
    }
}