using Akela.Behaviours;
using UnityEngine;

namespace Akela.Globals
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalFloatAsset Icon.png")]
    [CreateAssetMenu(fileName = "New Float", menuName = "Globals/Float", order = -99)]
    public sealed class GlobalFloat : GlobalBase<float>
    {

    }
}