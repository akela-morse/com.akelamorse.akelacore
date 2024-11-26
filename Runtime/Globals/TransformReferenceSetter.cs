using UnityEngine;

namespace Akela.Globals
{
    [AddComponentMenu("Globals/Transform Reference Setter", 1)]
    public sealed class TransformReferenceSetter : ReferenceSetterBase<GlobalTransformReference, Transform>
    {
#if UNITY_EDITOR
        private void Reset()
        {
            _value = transform;
        }
#endif
    }
}