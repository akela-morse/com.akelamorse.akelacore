using Akela.Signals;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using SignalType = Akela.Signals.SignalType;

namespace AkelaEditor.Events
{
	[CustomPropertyDrawer(typeof(SignalType))]
	internal class SignalTypeDrawer : PropertyDrawer
	{
		private static readonly string[] _eventTypes = AssetDatabase.FindAssets("t:" + nameof(Signal))
			.Select(x => AssetDatabase.LoadAssetAtPath<Signal>(AssetDatabase.GUIDToAssetPath(x)).Type.type)
			.Distinct()
			.ToArray();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (_eventTypes.Length == 0)
			{
				EditorGUI.LabelField(position, "No signals created yet.");
				return;
			}

			var displayLabel = property.displayName;

			if (property.propertyPath.Contains("Array"))
			{
				var num = property.propertyPath.LastIndexOf('[') + 1;
				var length = property.propertyPath.LastIndexOf(']') - num;
				var index = property.propertyPath.Substring(num, length);

				displayLabel = string.Format("Element {0}", index);
			}

			var actualProperty = property.FindPropertyRelative("type");
			var selectedIndex = Array.IndexOf(_eventTypes, actualProperty.stringValue);

			EditorGUI.BeginProperty(position, label, actualProperty);

			EditorGUI.BeginChangeCheck();

			var newIndex = EditorGUI.Popup(position, displayLabel, selectedIndex, _eventTypes);

			if (EditorGUI.EndChangeCheck())
				actualProperty.stringValue = _eventTypes[newIndex];

			EditorGUI.EndProperty();
		}
	}
}
