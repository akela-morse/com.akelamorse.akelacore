using Akela.Tools;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Tools
{
    [CustomPropertyDrawer(typeof(EnforceTypeAttribute))]
    public class EnforceTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                GUI.Label(position, "Use [EnforceType] with Object References");
                return;
            }

            var attr = (EnforceTypeAttribute)attribute;

            EditorGUI.BeginChangeCheck();

            var obj = EditorGUI.ObjectField(position, label, property.objectReferenceValue, attr.type, attr.allowSceneObjects);

            if (EditorGUI.EndChangeCheck())
            {
                if (obj)
                {
                    var type = obj.GetType();

                    if (!attr.type.IsAssignableFrom(type))
                    {
                        if (obj is GameObject gameObject)
                            obj = gameObject.GetComponent(attr.type);
                        else if (obj is Component component)
                            obj = component.gameObject.GetComponent(attr.type);
                        else
                            obj = null;
                    }
                }

                property.objectReferenceValue = obj;
            }
        }
    }
}