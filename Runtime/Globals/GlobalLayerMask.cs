using Akela.Behaviours;
using UnityEngine;

namespace Akela.Globals
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalLayerMaskAsset Icon.png")]
    [CreateAssetMenu(fileName = "New LayerMask", menuName = "Globals/LayerMask", order = -93)]
    public sealed class GlobalLayerMask : GlobalBase<LayerMask>
    {

    }
}