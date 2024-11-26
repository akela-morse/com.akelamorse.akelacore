using System.Collections.Generic;
using Akela.Tools;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Tools
{
    [CustomPropertyDrawer(typeof(EulerAnglesAttribute))]
    internal class EulerAnglesDrawer : PropertyDrawer
    {
        private Dictionary<string, Vector3> _internalVector3Value = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Quaternion)
            {
                GUI.Label(position, "Use [EulerAngles] with Quaternions");
                return;
            }

            _internalVector3Value.TryAdd(property.propertyPath, property.quaternionValue.eulerAngles);

            EditorGUI.BeginChangeCheck();

            _internalVector3Value[property.propertyPath] = EditorGUI.Vector3Field(position, label, _internalVector3Value[property.propertyPath]);

            if (EditorGUI.EndChangeCheck())
                property.quaternionValue = Quaternion.Euler(_internalVector3Value[property.propertyPath]);
        }
    }
}
