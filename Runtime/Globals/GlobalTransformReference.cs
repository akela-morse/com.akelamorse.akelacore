using UnityEngine;

namespace Akela.Globals
{
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalTransformAsset Icon.png")]
    [CreateAssetMenu(fileName = "New Transform Reference", menuName = "Globals/References/Transform", order = 11)]
    public sealed class GlobalTransformReference : GlobalReferenceBase<Transform>
    {

    }
}