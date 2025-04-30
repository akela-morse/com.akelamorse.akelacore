using Akela.Behaviours;
using UnityEngine;

namespace Akela.Globals
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalStringAsset Icon.png")]
    [CreateAssetMenu(fileName = "New String", menuName = "Globals/String", order = -98)]
    public sealed class GlobalString : GlobalBase<string>
    {

    }
}