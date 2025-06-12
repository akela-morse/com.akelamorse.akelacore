using Akela.ExtendedPhysics;
using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace AkelaEditor.ExtendedPhysics
{
    [CustomEditor(typeof(CustomCollider<>), true)]
    internal class CustomColliderEditor : Editor
    {
        internal static Color IDLE_COLOR = new Color(145f, 244f, 139f, 210f) / 191f;
        internal static Color DISABLED_COLOR = new Color(84f, 200f, 77f, 140f) / 191f;
        internal static Color TRANSPARENT = new(0f, 0f, 0f, 0f);

        private static readonly MethodInfo _getEditorToolFromType = typeof(Editor).Assembly
            .GetType("UnityEditor.EditorTools.EditorToolUtility")
            .GetMethod("GetCustomEditorToolsForType", BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly PropertyInfo _editorToolTypeProperty = typeof(Editor).Assembly
            .GetType("UnityEditor.EditorTools.EditorTypeAssociation")
            .GetProperty("editor", BindingFlags.Instance | BindingFlags.Public);


        private GUIContent _toolIcon;
        private Type _toolEditorType;
        private bool _layerOptionsFoldout;

        private void OnEnable()
        {
            _toolIcon = EditorGUIUtility.IconContent("EditCollider");

            var typeAssociation = ((IList)_getEditorToolFromType.Invoke(null, new[] { target.GetType() }))[0];
            _toolEditorType = (Type)_editorToolTypeProperty.GetValue(typeAssociation);
        }

        public override void OnInspectorGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var toolActive = ToolManager.activeToolType == _toolEditorType;

                EditorGUILayout.PrefixLabel("Edit Collider");

                var setToolActive = GUILayout.Toggle(toolActive, _toolIcon, "EditModeSingleButton", GUILayout.Width(33f), GUILayout.Height(21f));

                if (toolActive != setToolActive)
                {
                    if (setToolActive)
                        ToolManager.SetActiveTool(_toolEditorType);
                    else
                        ToolManager.RestorePreviousTool();
                }
            }

            serializedObject.Update();

            EditorGUILayout.Space(1f);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("isTrigger"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("providesContacts"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("material"));

            DrawPropertiesExcluding(serializedObject, "m_Script", "isTrigger", "providesContacts", "material", "layerOverridePriority", "includeLayers", "excludeLayers");

            _layerOptionsFoldout = EditorGUILayout.Foldout(_layerOptionsFoldout, "Layer Overrides");

            if (_layerOptionsFoldout)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(serializedObject.FindProperty("layerOverridePriority"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("includeLayers"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("excludeLayers"));

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}