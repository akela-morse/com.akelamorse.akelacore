using Akela.Behaviours;
using UnityEngine;

namespace Akela.Globals
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalColorAsset Icon.png")]
    [CreateAssetMenu(fileName = "New Color", menuName = "Globals/Color", order = -95)]
    public sealed class GlobalColor : GlobalBase<Color>
    {

    }
}