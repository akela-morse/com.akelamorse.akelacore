using Akela.Tools;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Tools
{
	[CustomPropertyDrawer(typeof(EulerAnglesAttribute))]
	internal class EulerAnglesDrawer : PropertyDrawer
	{
		private bool _didInitialise;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.Quaternion)
			{
				GUI.Label(position, "Use [EulerAngles] with Quaternions");
				return;
			}

			var attr = (EulerAnglesAttribute)attribute;

			if (!_didInitialise)
			{
				attr.internalVector3Value = property.quaternionValue.eulerAngles;
				_didInitialise = true;
			}

			EditorGUI.BeginChangeCheck();

			attr.internalVector3Value = EditorGUI.Vector3Field(position, label, attr.internalVector3Value);

			if (EditorGUI.EndChangeCheck())
				property.quaternionValue = Quaternion.Euler(attr.internalVector3Value);
		}
	}
}
