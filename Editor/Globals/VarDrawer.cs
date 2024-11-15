using Akela.Globals;
using AkelaEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Globals
{
    [CustomPropertyDrawer(typeof(Var<>))]
    internal class VarDrawer : PropertyDrawer
    {
        private static readonly Dictionary<Type, Type> containerTypes;
        
        static VarDrawer()
        {
            containerTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x =>  x.GetTypes())
                .Where(finalType => !finalType.IsGenericType && finalType
                    .GetBaseTypes()
                    .Any(baseType => baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(GlobalBase<>)))
                .ToDictionary(key => key.BaseType.GetGenericArguments()[0], elem => elem);
        }

        private bool _hasOpenedPicker;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var variableType = fieldInfo.FieldType.GetGenericArguments()[0];

            var localProperty = property.FindPropertyRelative("_localValue");
            var globalProperty = property.FindPropertyRelative("_globalValue");

            if (!containerTypes.TryGetValue(variableType, out var assetType))
            {
                EditorGUI.LabelField(position, $"Couldn't find a ScriptableObject to hold a global variable of type '{variableType}'");
                return;
            }

            if (_hasOpenedPicker && Event.current.type == EventType.ExecuteCommand)
            {
                if (Event.current.commandName == "ObjectSelectorUpdated")
                {
                    var selectedGlobal = EditorGUIUtility.GetObjectPickerObject();

                    globalProperty.serializedObject.Update();
                    globalProperty.objectReferenceValue = selectedGlobal;
                    globalProperty.serializedObject.ApplyModifiedProperties();
                }
                else if (Event.current.commandName == "ObjectSelectorClosed")
                {
                    _hasOpenedPicker = false;
                }
            }

            var fieldRect = new Rect(position.x, position.y, position.width - position.height - 1f, position.height);
            var buttonRect = new Rect(position.x + position.width - position.height, position.y, position.height, position.height);

            EditorGUI.BeginProperty(position, label, property);

            if (globalProperty.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(fieldRect, localProperty, label);

                if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("_Menu"), EditorStyles.iconButton))
                {
                    var controlId = (int)typeof(EditorGUIUtility).GetField("s_LastControlID", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                    var pickerMethod = typeof(EditorGUIUtility).GetMethod("ShowObjectPicker", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(assetType);

                    _hasOpenedPicker = true;

                    pickerMethod.Invoke(null, new object[] { null, false, string.Empty, controlId });
                }
            }
            else
            {
                EditorGUI.ObjectField(fieldRect, globalProperty, assetType, label);

                if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("clear"), EditorStyles.iconButton))
                {
                    globalProperty.serializedObject.Update();
                    globalProperty.objectReferenceValue = null;
                    globalProperty.serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
