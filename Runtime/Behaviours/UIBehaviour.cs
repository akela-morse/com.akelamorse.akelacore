using UnityEngine;

namespace Akela.Behaviours
{
    public abstract class UIBehaviour : MonoBehaviour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Field hiding")]
        public new RectTransform transform => (RectTransform)base.transform;
    }
}