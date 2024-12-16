using Akela.ExtendedPhysics;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.ExtendedPhysics
{
    [CustomEditor(typeof(Raycaster))]
    internal class RaycasterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_updateType"));

            EditorGUILayout.Space();

            DrawGeneralOptions();

            EditorGUILayout.Space();

            DrawShapeOptions();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGeneralOptions()
        {
            EditorGUILayout.LabelField("General Options", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_direction"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_castSpace"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_layerMask"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_triggerInteraction"));

            EditorGUILayout.Space();

            var hitProperty = serializedObject.FindProperty("_registerMultipleHits");

            EditorGUILayout.PropertyField(hitProperty);

            if (hitProperty.boolValue)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxNumberOfHits"));
        }

        private void DrawShapeOptions()
        {
            EditorGUILayout.LabelField("Shape", EditorStyles.boldLabel);

            var shapeProperty = serializedObject.FindProperty("_shape");

            EditorGUILayout.PropertyField(shapeProperty);

            switch ((Raycaster.RaycastShape)shapeProperty.enumValueIndex)
            {
                case Raycaster.RaycastShape.Sphere:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_radius"));
                    break;

                case Raycaster.RaycastShape.Box:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_boxSize"), new GUIContent("Size"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_orientation"));
                    break;

                case Raycaster.RaycastShape.Capsule:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_radius"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_capsuleHeight"), new GUIContent("Height"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_orientation"));
                    break;
            }
        }
    }
}