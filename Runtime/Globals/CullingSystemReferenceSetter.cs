using Akela.Optimisations;
using UnityEngine;

namespace Akela.Globals
{
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/CullingSystemSetter Icon.png")]
    [AddComponentMenu("Globals/Culling System Reference Setter", 3)]
    public sealed class CullingSystemReferenceSetter : ReferenceSetterBase<GlobalCullingSystemReference, CullingSystem>
    {
#if UNITY_EDITOR
        private void Reset()
        {
            _value = GetComponent<CullingSystem>();
        }
#endif
    }
}