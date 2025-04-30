using Akela.Behaviours;
using UnityEngine;

namespace Akela.Globals
{
    [HideScriptField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/GlobalCameraAsset Icon.png")]
    [CreateAssetMenu(fileName = "New Camera Reference", menuName = "Globals/References/Camera", order = 12)]
    public sealed class GlobalCameraReference : GlobalReferenceBase<Camera>
    {

    }
}