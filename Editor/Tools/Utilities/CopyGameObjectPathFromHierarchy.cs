using UnityEditor;

namespace AkelaEditor.Tools
{
    public static class CopyGameObjectPathFromHierarchy
    {
        [MenuItem("GameObject/Copy Path", false, 11)]
        static void CopyPath()
        {
            var currentGameObject = Selection.activeGameObject;

            if (currentGameObject == null)
                return;

            var path = currentGameObject.name;

            while (currentGameObject.transform.parent != null)
            {
                currentGameObject = currentGameObject.transform.parent.gameObject;

                path = $"{currentGameObject.name}/{path}";
            }

            EditorGUIUtility.systemCopyBuffer = path;
        }

        /// <summary>
        /// Only allow path copying if 1 object is selected.
        /// </summary>
        [MenuItem("GameObject/Copy Path", true)]
        static bool CopyPathValidation() => Selection.gameObjects.Length == 1;
    }
}