using UnityEditor;

namespace AkelaEditor.Tools
{
    public static class SerializedPropertyExtensions
    {
        public static void ResetToDefaultValue(this SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Character:
                    property.intValue = default;
                    break;

                case SerializedPropertyType.Boolean:
                    property.boolValue = default;
                    break;

                case SerializedPropertyType.Float:
                    property.floatValue = default;
                    break;

                case SerializedPropertyType.String:
                    property.stringValue = default;
                    break;

                case SerializedPropertyType.Color:
                    property.colorValue = default;
                    break;

                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = default;
                    break;

                case SerializedPropertyType.Enum:
                    property.enumValueIndex = 0;
                    break;

                case SerializedPropertyType.Vector2:
                    property.vector2Value = default;
                    break;

                case SerializedPropertyType.Vector3:
                    property.vector3Value = default;
                    break;

                case SerializedPropertyType.Vector4:
                    property.vector4Value = default;
                    break;

                case SerializedPropertyType.Rect:
                    property.rectValue = default;
                    break;

                case SerializedPropertyType.ArraySize:
                    property.arraySize = 0;
                    break;

                case SerializedPropertyType.AnimationCurve:
                    property.animationCurveValue = default;
                    break;

                case SerializedPropertyType.Bounds:
                    property.boundsValue = default;
                    break;

                case SerializedPropertyType.Quaternion:
                    property.quaternionValue = default;
                    break;

                case SerializedPropertyType.ExposedReference:
                    property.exposedReferenceValue = default;
                    break;

                case SerializedPropertyType.Vector2Int:
                    property.vector2IntValue = default;
                    break;

                case SerializedPropertyType.Vector3Int:
                    property.vector3IntValue = default;
                    break;

                case SerializedPropertyType.RectInt:
                    property.rectIntValue = default;
                    break;

                case SerializedPropertyType.BoundsInt:
                    property.boundsIntValue = default;
                    break;

                case SerializedPropertyType.ManagedReference:
                    property.managedReferenceValue = default;
                    break;
            }
        }
    }
}
