using Akela.Behaviours;
using UnityEngine;

namespace Akela.Globals
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalVector4Asset Icon.png")]
    [CreateAssetMenu(fileName = "New Vector4", menuName = "Globals/Vector4", order = -95)]
    public sealed class GlobalVector4 : GlobalBase<Vector4>
    {

    }
}