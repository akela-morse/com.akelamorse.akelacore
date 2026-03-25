using System.Linq;
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
            AssemblyReloadEvents.afterAssemblyReload += UpdateEverything;
        }

        private static void UpdateEverything()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            var notifiers = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<INotifyUpdatedInEditor>();

            foreach (var notifier in notifiers)
                notifier.UpdatedInEditor();
        }

        private static void ChangesPublished(ref ObjectChangeEventStream stream)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            for (var i = 0; i < stream.length; ++i)
            {
                var type = stream.GetEventType(i);

                switch (type)
                {
                    case ObjectChangeKind.CreateGameObjectHierarchy:
                    {
                        stream.GetCreateGameObjectHierarchyEvent(i, out var data);

                        var gameObject = EditorUtility.InstanceIDToObject(data.instanceId) as GameObject;

                        if (gameObject)
                        {
                            foreach (var notifier in gameObject.GetComponents<INotifyUpdatedInEditor>())
                                notifier.UpdatedInEditor();
                        }

                        break;
                    }

                    case ObjectChangeKind.ChangeGameObjectStructure:
                    {
                        stream.GetChangeGameObjectStructureEvent(i, out var data);

                        var gameObject = EditorUtility.InstanceIDToObject(data.instanceId) as GameObject;

                        if (gameObject)
                        {
                            foreach (var notifier in gameObject.GetComponents<INotifyUpdatedInEditor>())
                                notifier.UpdatedInEditor();
                        }

                        break;
                    }

                    case ObjectChangeKind.ChangeGameObjectOrComponentProperties:
                    {
                        stream.GetChangeGameObjectOrComponentPropertiesEvent(i, out var data);

                        if (EditorUtility.InstanceIDToObject(data.instanceId) is INotifyUpdatedInEditor notifier)
                            notifier.UpdatedInEditor();

                        break;
                    }

                    case ObjectChangeKind.ChangeAssetObjectProperties:
                    {
                        stream.GetChangeAssetObjectPropertiesEvent(i, out var data);

                        if (EditorUtility.InstanceIDToObject(data.instanceId) is INotifyUpdatedInEditor notifier)
                            notifier.UpdatedInEditor();

                        break;
                    }

                    case ObjectChangeKind.ChangeChildrenOrder:
                    {
                        stream.GetChangeChildrenOrderEvent(i, out var data);

                        var gameObject = EditorUtility.InstanceIDToObject(data.instanceId) as GameObject;

                        if (gameObject)
                        {
                            foreach (var notifier in gameObject.GetComponentsInChildren<INotifyUpdatedInEditor>())
                                notifier.UpdatedInEditor();
                        }

                        break;
                    }

                    case ObjectChangeKind.UpdatePrefabInstances:
                    {
                        stream.GetUpdatePrefabInstancesEvent(i, out var data);

                        for (var j = 0; j < data.instanceIds.Length; ++j)
                        {
                            var gameObject = EditorUtility.InstanceIDToObject(data.instanceIds[j]) as GameObject;

                            if (gameObject)
                            {
                                foreach (var notifier in gameObject.GetComponentsInChildren<INotifyUpdatedInEditor>())
                                    notifier.UpdatedInEditor();
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}