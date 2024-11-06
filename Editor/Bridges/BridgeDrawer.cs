using Akela.Bridges;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Bridges
{
	[CustomPropertyDrawer(typeof(IBridge), true)]
	internal class BridgeDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_internalValue"));
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var actualProperty = property.FindPropertyRelative("_internalValue");

			EditorGUI.PropertyField(position, actualProperty, new GUIContent(property.displayName));
		}
	}
}
