using Akela.Behaviours;
using UnityEngine;

namespace Akela.Motion
{
    [AddComponentMenu("Motion/Continuous Rotation", 4)]
    [TickOptions(TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate, TickUpdateType.AnimatorMove)]
    [HideScriptField, DisallowMultipleComponent]
    public class ContinuousRotation : TickBehaviour
    {
        [SerializeField] Vector3 _speed;

        protected override void Tick(float deltaTime)
        {
            transform.Rotate(_speed * deltaTime);
        }
    }
}
