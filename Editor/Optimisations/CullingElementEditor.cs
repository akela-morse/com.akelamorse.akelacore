using Akela.Optimisations;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AkelaEditor.Optimisations
{
    [CustomEditor(typeof(CullingElement))]
    internal class CullingElementEditor : Editor
    {
        private readonly SphereBoundsHandle _sphereHandle = new();

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_system"));

            EditorGUILayout.Space();

            DrawPropertiesExcluding(serializedObject, "m_Script", "_system");

            serializedObject.ApplyModifiedProperties();

            if (Application.isPlaying)
            {
                var element = (CullingElement)target;

                EditorGUILayout.Space();

                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.Toggle("Is Visible", element.IsVisible);
                    EditorGUILayout.IntField("Distance Band", element.CurrentDistanceBand);
                }
            }
        }

        private void OnSceneGUI()
        {
            var centerProperty = serializedObject.FindProperty("_sphereCenter");
            var radiusProperty = serializedObject.FindProperty("_sphereRadius");

            using (new Handles.DrawingScope(new(.85f, .67f, 1f), ((CullingElement)target).transform.localToWorldMatrix))
            {
                _sphereHandle.axes = PrimitiveBoundsHandle.Axes.All;
                _sphereHandle.center = centerProperty.vector3Value;
                _sphereHandle.radius = radiusProperty.floatValue;

                EditorGUI.BeginChangeCheck();

                _sphereHandle.DrawHandle();

                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.Update();

                    centerProperty.vector3Value = _sphereHandle.center;
                    radiusProperty.floatValue = _sphereHandle.radius;

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
