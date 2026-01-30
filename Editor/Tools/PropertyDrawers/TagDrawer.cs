using System;
using Akela.Tools;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AkelaEditor.Tools
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    internal class TagDrawer : PropertyDrawer
    {
        private static string[] _tags;
        private static GUIContent[] _tagContents;

        private static void FetchTags()
        {
            _tags = InternalEditorUtility.tags;

            if (_tagContents == null || _tagContents.Length != _tags.Length)
                _tagContents = Array.ConvertAll(_tags, x => new GUIContent(x));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                GUI.Label(position, "Use [Tag] with String fields.");
                return;
            }

            FetchTags();

            var currentIndex = Array.IndexOf(_tags, property.stringValue);

            int newIndex;

            if (currentIndex < 0)
                newIndex = 0;
            else
                newIndex = EditorGUI.Popup(position, label, currentIndex, _tagContents);

            if (currentIndex != newIndex)
                property.stringValue = _tags[newIndex];
        }
    }
}