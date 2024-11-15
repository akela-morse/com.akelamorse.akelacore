using UnityEngine;

namespace Akela.ExtendedPhysics
{
    public interface ICustomCollider
    {
        bool IsBindingCollider(Collider collider);
    }
}
