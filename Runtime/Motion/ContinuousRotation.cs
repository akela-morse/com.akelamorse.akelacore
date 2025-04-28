using Akela.Behaviours;
using UnityEngine;

namespace Akela.Motion
{
    [HideScriptField, DisallowMultipleComponent]
    [TickOptions(TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate, TickUpdateType.AnimatorMove)]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/ContinuousRotation Icon.png")]
    [AddComponentMenu("Motion/Continuous Rotation", 4)]
    public class ContinuousRotation : TickBehaviour
    {
        [SerializeField] Vector3 _speed;

        protected override void Tick(float deltaTime)
        {
            transform.Rotate(_speed * deltaTime);
        }
    }
}