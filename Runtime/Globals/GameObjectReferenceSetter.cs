using UnityEngine;

namespace Akela.Globals
{
    [AddComponentMenu("Globals/GameObject Reference Setter", 8)]
    public sealed class GameObjectReferenceSetter : ReferenceSetterBase<GlobalGameObjectReference, GameObject>
    {
#if UNITY_EDITOR
        private void Reset()
        {
            _value = gameObject;
        }
#endif
    }
}