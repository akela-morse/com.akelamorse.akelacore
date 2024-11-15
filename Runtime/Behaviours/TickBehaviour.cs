using UnityEngine;

namespace Akela.Behaviours
{
    public abstract class TickBehaviour : MonoBehaviour
    {
        public enum TickUpdateType
        {
            None,
            Update,
            LateUpdate,
            FixedUpdate,
            AnimatorMove
        }

        #region Component Fields
        [SerializeField] protected TickUpdateType _updateType = TickUpdateType.Update;
        #endregion

#if UNITY_EDITOR
        [System.NonSerialized] private double _editorTime;

        public void ForceResetEditorDeltaTime()
        {
            _editorTime = UnityEditor.EditorApplication.timeSinceStartup;
        }
#endif

        protected abstract void Tick(float deltaTime);

        #region Component Messages
        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var deltaTime = UnityEditor.EditorApplication.timeSinceStartup - _editorTime;
                Tick((float)deltaTime);
                _editorTime = UnityEditor.EditorApplication.timeSinceStartup;
                return;
            }
#endif

            if (_updateType != TickUpdateType.Update)
                return;

            Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            if (_updateType != TickUpdateType.LateUpdate)
                return;

            Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            
            if (_updateType != TickUpdateType.FixedUpdate)
                return;

            Tick(Time.fixedDeltaTime);
        }

        private void OnAnimatorMove()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            
            if (_updateType != TickUpdateType.AnimatorMove)
                return;

            Tick(Time.deltaTime);
        }
        #endregion
    }
}
