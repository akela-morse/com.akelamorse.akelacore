using UnityEngine;

namespace Akela.Optimisations
{
    [AddComponentMenu("Optimisation/Component Cull", 4)]
    [RequireComponent(typeof(CullingElement))]
    public class ComponentCull : MonoBehaviour, ICullingMessageReceiver
    {
        #region Component Fields
        [Tooltip("If d < x components will be active\nIf x <= d < y components will be inactive if culled\nIf y <= d components will be inactive")]
#if AKELA_VINSPECTOR
        [VInspector.MinMaxSlider(0, 8)]
#endif
        [SerializeField] Vector2Int _distanceBandRange = new(1, 4);
        [SerializeField] Component[] _componentsToCull;
        #endregion

        private bool _currentStateOfComponents;
        private CullingElement _cullingElement;

        public void OnCullingElementInvisible()
        {
            SetComponentState(_cullingElement.CurrentDistanceBand < _distanceBandRange.x);
        }

        public void OnCullingElementVisible()
        {
            SetComponentState(_cullingElement.CurrentDistanceBand <= _distanceBandRange.y);
        }

        public void OnDistanceBandChanges(int _, int newBand)
        {
            if (newBand < _distanceBandRange.x)
                SetComponentState(true);
            else if (newBand < _distanceBandRange.y)
                SetComponentState(_cullingElement.IsVisible);
            else
                SetComponentState(false);
        }

        #region Component Messages
        private void Awake()
        {
            _cullingElement = GetComponent<CullingElement>();
            _currentStateOfComponents = true;
        }
        #endregion

        #region Private Methods
        private void SetComponentState(bool state)
        {
            if (state == _currentStateOfComponents)
                return;

            foreach (var component in _componentsToCull)
            {
                if (!component)
                    continue;

                switch (component)
                {
                    case Rigidbody rb:
                        rb.isKinematic = !state;
                        break;

                    case Collider c:
                        c.enabled = state;
                        break;

                    case Behaviour behaviour:
                        behaviour.enabled = state;
                        break;
                }
            }

            _currentStateOfComponents = state;
        }
        #endregion
    }
}