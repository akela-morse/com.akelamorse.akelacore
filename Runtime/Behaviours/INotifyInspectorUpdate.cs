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
#if UNITY_EDITOR
        void UpdatedInInspector();

        void ISerializationCallbackReceiver.OnAfterDeserialize() { }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (
                EditorApplication.isPlayingOrWillChangePlaymode ||
                EditorApplication.isCompiling ||
                EditorApplication.isUpdating ||
                this is not Component ||
                !(Component)this ||
                !((Component)this).gameObject ||
                !((Component)this).gameObject.scene.IsValid() ||
                !((Component)this).gameObject.scene.GetPhysicsScene().IsValid()
            )
                return;

            UpdatedInInspector();
        }
#endif
    }
}