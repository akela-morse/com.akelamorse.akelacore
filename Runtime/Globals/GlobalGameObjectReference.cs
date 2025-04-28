using UnityEngine;

namespace Akela.Globals
{
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalGameObjectAsset Icon.png")]
    [CreateAssetMenu(fileName = "New GameObject Reference", menuName = "Globals/References/GameObject", order = 10)]
    public sealed class GlobalGameObjectReference : GlobalReferenceBase<GameObject>
    {

    }
}