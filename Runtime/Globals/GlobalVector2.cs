using Akela.Behaviours;
using UnityEngine;

namespace Akela.Globals
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalVector2Asset Icon.png")]
    [CreateAssetMenu(fileName = "New Vector2", menuName = "Globals/Vector2", order = -97)]
    public sealed class GlobalVector2 : GlobalBase<Vector2>
    {

    }
}