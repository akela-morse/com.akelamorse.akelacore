using Akela.Tools;
using UnityEngine;

namespace Akela.ExtendedPhysics
{
    [AddComponentMenu("New script/Sub Collider")]
    public sealed class SubCollider : MonoBehaviour
    {
        public Collider colliderComponent;
        public Component bindingCollider;

        public void DestroyIfUnbound()
        {
            if (bindingCollider == null || !((ICustomCollider)bindingCollider).IsBindingCollider(colliderComponent))
                gameObject.PlaymodeAgnosticDestroy();
        }

        private void OnTriggerEnter(Collider other)
        {
            bindingCollider.SendMessage(nameof(OnTriggerEnter), other, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerStay(Collider other)
        {
            bindingCollider.SendMessage(nameof(OnTriggerStay), other, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTriggerExit(Collider other)
        {
            bindingCollider.SendMessage(nameof(OnTriggerExit), other, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionEnter(Collision collision)
        {
            bindingCollider.SendMessage(nameof(OnCollisionEnter), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionStay(Collision collision)
        {
            bindingCollider.SendMessage(nameof(OnCollisionStay), collision, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionExit(Collision collision)
        {
            bindingCollider.SendMessage(nameof(OnCollisionExit), collision, SendMessageOptions.DontRequireReceiver);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            hideFlags = HideFlags.NotEditable;

            if (colliderComponent)
                colliderComponent.hideFlags = HideFlags.NotEditable;

            gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
        }
#endif
    }
}