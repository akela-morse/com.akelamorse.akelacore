using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace AkelaEditor.Tools
{
    public abstract class ComponentOverlay<T> : Overlay, ITransientOverlay where T : MonoBehaviour
    {
        public bool visible { get; private set; }

        protected T target;
        protected SerializedObject serializedObject;

        protected virtual void OnBecomeActive() { }
        protected virtual void OnBecomeInactive() { }

        public override void OnCreated()
        {
            Selection.selectionChanged += CheckComponentIsAvailable;
        }

        public override void OnWillBeDestroyed()
        {
            if (visible)
                OnBecomeInactive();

            Selection.selectionChanged -= CheckComponentIsAvailable;
        }

        private void CheckComponentIsAvailable()
        {
            T newComponent = null;
            
            var newState =
                Selection.activeGameObject &&
                Selection.activeGameObject.TryGetComponent(out newComponent) &&
                newComponent.isActiveAndEnabled;

            if (newState != visible)
            {
                visible = newState;

                if (visible)
                {
                    target = newComponent;
                    serializedObject = new SerializedObject(target);
                    
                    OnBecomeActive();
                }
                else
                {
                    OnBecomeInactive();
                    
                    target = null;
                    serializedObject = null;
                }
            }
            else if (newComponent != target)
            {
                if (target)
                    OnBecomeInactive();
                
                target = newComponent;
                serializedObject = new SerializedObject(target);
                
                if (target)
                    OnBecomeActive();
            }
        }
    }
}
