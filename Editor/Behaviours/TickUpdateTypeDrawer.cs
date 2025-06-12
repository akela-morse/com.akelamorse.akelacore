using Akela.Behaviours;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static Akela.Behaviours.TickBehaviour;

namespace AkelaEditor
{
    [CustomPropertyDrawer(typeof(TickUpdateType))]
    internal class TickUpdateTypeDrawer : PropertyDrawer
    {
        static readonly TickUpdateType[] DEFAULT_OPTIONS = new[] { TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var optionsAttr = property.serializedObject.targetObject.GetType().GetCustomAttribute<TickOptionsAttribute>();

            var allowedOptions = optionsAttr != null ? optionsAttr.AllowedUpdateTypes : DEFAULT_OPTIONS;

            var currentlySelectedOption = (TickUpdateType)property.enumValueIndex;
            var currentlySelectedIndex = Array.IndexOf(allowedOptions, currentlySelectedOption);

            if (!allowedOptions.Contains(currentlySelectedOption))
            {
                currentlySelectedOption = allowedOptions.First();
                property.enumValueIndex = (int)currentlySelectedOption;

                return;
            }

            EditorGUI.BeginProperty(position, label, property);
            var newIndex = EditorGUI.Popup(position, "Refresh Every", currentlySelectedIndex, Array.ConvertAll(allowedOptions, GetEnumName));
            EditorGUI.EndProperty();

            if (newIndex != currentlySelectedIndex)
                property.enumValueIndex = (int)allowedOptions[newIndex];
        }

        private static string GetEnumName(TickUpdateType tickUpdateType)
        {
            if (tickUpdateType == TickUpdateType.None)
                return "Don't Refresh";

            return ObjectNames.NicifyVariableName(Enum.GetName(typeof(TickUpdateType), tickUpdateType));
        }
    }
}