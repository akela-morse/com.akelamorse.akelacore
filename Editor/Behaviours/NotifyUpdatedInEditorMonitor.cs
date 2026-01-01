using Akela.Behaviours;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor
{
    [InitializeOnLoad]
    internal static class NotifyUpdatedInEditorMonitor
    {
        static NotifyUpdatedInEditorMonitor()
        {
            ObjectChangeEvents.changesPublished += ChangesPublished;
        }

        private static void ChangesPublished(ref ObjectChangeEventStream stream)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            for (var i = 0; i < stream.length; ++i)
            {
                var type = stream.GetEventType(i);

                if (type == ObjectChangeKind.CreateGameObjectHierarchy)
                {
                    stream.GetCreateGameObjectHierarchyEvent(i, out var data);

                    var gameObject = EditorUtility.InstanceIDToObject(data.instanceId) as GameObject;

                    if (gameObject)
                    {
                        foreach (var notifier in gameObject.GetComponents<INotifyUpdatedInEditor>())
                            notifier.UpdatedInEditor();
                    }
                }
                else if (type == ObjectChangeKind.ChangeGameObjectStructure)
                {
                    stream.GetChangeGameObjectStructureEvent(i, out var data);

                    var gameObject = EditorUtility.InstanceIDToObject(data.instanceId) as GameObject;

                    if (gameObject)
                    {
                        foreach (var notifier in gameObject.GetComponents<INotifyUpdatedInEditor>())
                            notifier.UpdatedInEditor();
                    }
                }
                else if (type == ObjectChangeKind.ChangeGameObjectOrComponentProperties)
                {
                    stream.GetChangeGameObjectOrComponentPropertiesEvent(i, out var data);

                    if (EditorUtility.InstanceIDToObject(data.instanceId) is INotifyUpdatedInEditor notifier)
                        notifier.UpdatedInEditor();
                }
                else if (type == ObjectChangeKind.ChangeAssetObjectProperties)
                {
                    stream.GetChangeAssetObjectPropertiesEvent(i, out var data);

                    if (EditorUtility.InstanceIDToObject(data.instanceId) is INotifyUpdatedInEditor notifier)
                        notifier.UpdatedInEditor();
                }
            }
        }
    }
}