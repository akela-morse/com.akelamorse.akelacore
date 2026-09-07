using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Akela.Behaviours
{
    public interface INotifyInspectorUpdate
#if UNITY_EDITOR
        : ISerializationCallbackReceiver
#endif
    {
        void UpdatedInInspector();

#if UNITY_EDITOR
        void ISerializationCallbackReceiver.OnAfterDeserialize() { }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (
                EditorApplication.isPlayingOrWillChangePlaymode ||
                EditorApplication.isCompiling ||
                EditorApplication.isUpdating ||
                this is not Component and not ScriptableObject ||
                this is Component component && (
                    !component ||
                    !component.gameObject ||
                    !component.gameObject.scene.IsValid() ||
                    !component.gameObject.scene.GetPhysicsScene().IsValid()
                ) ||
                this is ScriptableObject scriptableObject && (
                    !scriptableObject
                )
            )
                return;

            UpdatedInInspector();
        }
#endif
    }
}