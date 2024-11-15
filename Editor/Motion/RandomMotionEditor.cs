using Akela.Motion;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AkelaEditor.Motion
{
    [CustomEditor(typeof(RandomMotion))]
    internal class RandomMotionEditor : Editor
    {
        private readonly SphereBoundsHandle _sphereHandle = new();

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var radiusProperty = serializedObject.FindProperty("_radius");

            if (radiusProperty.floatValue < 0f)
                return;

            using (new Handles.DrawingScope(new Color(244f, 139f, 101f, 210f) / 191f, ((RandomMotion)target).transform.localToWorldMatrix))
            {
                _sphereHandle.axes = PrimitiveBoundsHandle.Axes.All;
                _sphereHandle.center = Vector3.zero;
                _sphereHandle.radius = radiusProperty.floatValue;

                EditorGUI.BeginChangeCheck();

                _sphereHandle.DrawHandle();

                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.Update();

                    radiusProperty.floatValue = _sphereHandle.radius;

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
