using Akela.Signals;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Signals
{
    [CustomEditor(typeof(ObjectFunctions))]
    public class ObjectFunctionsEditor : Editor
    {
        private int newlyCreatedIndex = -1;
        private int setForDeletion = -1;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var keysProp = serializedObject.FindProperty("_keys");
            var valuesProp = serializedObject.FindProperty("_values");

            for (var i = 0; i < keysProp.arraySize; ++i)
            {
                var key = keysProp.GetArrayElementAtIndex(i);
                var value = valuesProp.GetArrayElementAtIndex(i);

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUI.SetNextControlName("NameControl" + i);
                        EditorGUILayout.PropertyField(key, GUIContent.none);

                        if (GUILayout.Button("Delete", GUILayout.Width(50)))
                            setForDeletion = i;
                    }

                    EditorGUILayout.PropertyField(value, new GUIContent(key.stringValue));
                }
            }

            if (newlyCreatedIndex != -1)
            {
                EditorGUI.FocusTextInControl("NameControl" + newlyCreatedIndex);
                newlyCreatedIndex = -1;
            }

            if (setForDeletion != -1)
            {
                keysProp.DeleteArrayElementAtIndex(setForDeletion);
                valuesProp.DeleteArrayElementAtIndex(setForDeletion);

                setForDeletion = -1;
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Add Function", GUILayout.Width(100), GUILayout.Height(25)))
            {
                newlyCreatedIndex = keysProp.arraySize;

                keysProp.InsertArrayElementAtIndex(newlyCreatedIndex);
                valuesProp.InsertArrayElementAtIndex(newlyCreatedIndex);

                var newNameProp = keysProp.GetArrayElementAtIndex(newlyCreatedIndex);
                newNameProp.stringValue = "<New Function>";

                var newFunctionProp = valuesProp.GetArrayElementAtIndex(newlyCreatedIndex);
                var bridgeInternalValue = newFunctionProp.FindPropertyRelative("_internalValue");

                var calls = bridgeInternalValue.FindPropertyRelative("m_PersistentCalls"); // UnityEvent
                calls ??= bridgeInternalValue.FindPropertyRelative("_PersistentCalls"); // UltEvent

                for (var i = calls.arraySize - 1; i >= 0; --i)
                    calls.DeleteArrayElementAtIndex(i);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
