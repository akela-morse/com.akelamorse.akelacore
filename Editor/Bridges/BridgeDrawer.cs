using Akela.Bridges;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AkelaEditor.Bridges
{
	[CustomPropertyDrawer(typeof(IBridge), true)]
	internal class BridgeDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var actualProperty = property.FindPropertyRelative("_internalValue");

			var container = new VisualElement();
			container.Add(new PropertyField(actualProperty, property.displayName));

			return container;
		}
	}
}
