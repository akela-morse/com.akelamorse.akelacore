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
            if (property.propertyType != SerializedPropertyType.ObjectReference && (property.propertyType != SerializedPropertyType.Generic || !property.isArray))
                return;

            EditorGUI.DrawRect(new Rect(position) { x = 0f, width = 2f }, Color.darkOrchid);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndDisabledGroup();

            if (Event.current.type != EventType.Repaint)
                return;

            var type =
                fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() :
                fieldInfo.FieldType.IsGenericType ? fieldInfo.FieldType.GenericTypeArguments[0] :
                fieldInfo.FieldType;

            if (attribute is FromThisAttribute)
            {
                if (property.isArray)
                {
                    var components = ((MonoBehaviour)property.serializedObject.targetObject).GetComponents(type);
                    property.arraySize = components.Length;

                    for (var i = 0; i < components.Length; ++i)
                        property.GetArrayElementAtIndex(i).objectReferenceValue = components[i];
                }
                else
                {
                    property.objectReferenceValue = ((MonoBehaviour)property.serializedObject.targetObject).GetComponent(type);
                }
            }
            else if (attribute is FromParentsAttribute)
            {
                if (property.isArray)
                {
                    var components = ((MonoBehaviour)property.serializedObject.targetObject).GetComponentsInParent(type);
                    property.arraySize = components.Length;

                    for (var i = 0; i < components.Length; ++i)
                        property.GetArrayElementAtIndex(i).objectReferenceValue = components[i];
                }
                else
                {
                    property.objectReferenceValue = ((MonoBehaviour)property.serializedObject.targetObject).GetComponentInParent(type);
                }
            }
            else if (attribute is FromChildrenAttribute)
            {
                if (property.isArray)
                {
                    var components = ((MonoBehaviour)property.serializedObject.targetObject).GetComponentsInChildren(type);
                    property.arraySize = components.Length;

                    for (var i = 0; i < components.Length; ++i)
                        property.GetArrayElementAtIndex(i).objectReferenceValue = components[i];
                }
                else
                {
                    property.objectReferenceValue = ((MonoBehaviour)property.serializedObject.targetObject).GetComponentInChildren(type);
                }
            }
        }
    }
}