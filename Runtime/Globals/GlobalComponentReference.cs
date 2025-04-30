using Akela.Behaviours;
using UnityEngine;

namespace Akela.Globals
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalComponentAsset Icon.png")]
    [CreateAssetMenu(fileName = "New Component Reference", menuName = "Globals/References/Component", order = 14)]
    public sealed class GlobalComponentReference : GlobalReferenceBase<Component>
    {

    }
}