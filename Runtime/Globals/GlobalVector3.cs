using Akela.Behaviours;
using UnityEngine;

namespace Akela.Globals
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalVector3Asset Icon.png")]
    [CreateAssetMenu(fileName = "New Vector3", menuName = "Globals/Vector3", order = -96)]
    public sealed class GlobalVector3 : GlobalBase<Vector3>
    {

    }
}