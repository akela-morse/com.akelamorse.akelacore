using Akela.Behaviours;
using Akela.Signals;
using Akela.Globals;
using UnityEngine;

namespace Akela.Optimisations
{
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/CullingElement Icon.png")]
    [AddComponentMenu("Optimisation/Culling Element", 1)]
    public class CullingElement : TickBehaviour, ICullingElement
    {
        #region Component Fields
        [SerializeField] Var<CullingSystem> _system;
        [Space]
        [SerializeField] Vector3 _sphereCenter;
        [SerializeField] float _sphereRadius;
        #endregion

        private int _elementId;
        private MessageBroadcaster<ICullingMessageReceiver> _messageBroadcaster;

        public bool IsVisible { get; private set; }
        public int CurrentDistanceBand { get; private set; }

        public CullingSystem CullingSystem => _system;

        void ICullingElement.IndexChanged(int index)
        {
            _elementId = index;
        }

        void ICullingElement.InitialState(bool visible, int distanceBand)
        {
            IsVisible = visible;
            CurrentDistanceBand = distanceBand;

            if (IsVisible)
                _messageBroadcaster.Dispatch(x => x.OnCullingElementVisible());
            else
                _messageBroadcaster.Dispatch(x => x.OnCullingElementInvisible());

            _messageBroadcaster.Dispatch(x => x.OnDistanceBandChanges(-1, CurrentDistanceBand));
        }

        void ICullingElement.StateChanged(CullingGroupEvent data)
        {
            IsVisible = data.isVisible;
            CurrentDistanceBand = data.currentDistance;

            if (data.hasBecomeVisible)
                _messageBroadcaster.Dispatch(x => x.OnCullingElementVisible());

            if (data.hasBecomeInvisible)
                _messageBroadcaster.Dispatch(x => x.OnCullingElementInvisible());

            if (data.previousDistance != data.currentDistance)
                _messageBroadcaster.Dispatch(x => x.OnDistanceBandChanges(data.previousDistance, data.currentDistance));
        }

        #region Component Messages
        private void Awake()
        {
            _messageBroadcaster = new(gameObject);
        }

        private void Start()
        {
            _elementId = _system.Value.RegisterElement(this, new(_sphereCenter.x, _sphereCenter.y, _sphereCenter.z, _sphereRadius));
        }

        protected override void Tick(float deltaTime)
        {
            _system.Value.UpdateSpherePosition(_elementId, transform.TransformPoint(_sphereCenter));
        }

        private void OnDestroy()
        {
            _system.Value.UnregisterElement(_elementId);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _sphereRadius = 2f;
        }

        private void OnValidate()
        {
            if (_sphereRadius < 0f)
                _sphereRadius = 0f;
        }
#endif
        #endregion
    }
}