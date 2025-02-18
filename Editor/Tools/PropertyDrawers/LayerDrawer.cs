using Akela.Tools;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Tools
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    internal class LayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                GUI.Label(position, "Use [EulerAngles] with Int32 fields.");
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}