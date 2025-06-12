using UnityEngine;

namespace Akela.Behaviours
{
    public abstract class UIBehaviour : UnityEngine.EventSystems.UIBehaviour
    {
        public new RectTransform transform => (RectTransform)base.transform;
    }
}