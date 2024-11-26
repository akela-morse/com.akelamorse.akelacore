using Akela.Behaviours;
using Akela.Tools;
using System;
using UnityEngine;

namespace Akela.ExtendedPhysics
{
    [ExecuteAlways]
    public abstract class CustomCollider<T> : MonoBehaviour, ICustomCollider, INotifySerializedFieldChanged where T: Collider
    {
        public bool isTrigger;
        public bool providesContacts;
        public PhysicsMaterial material;
        public int layerOverridePriority;
        public LayerMask includeLayers;
        public LayerMask excludeLayers;

        [SerializeField, HideInInspector] protected T[] _subColliders;

        protected Bounds Bounds { get; private set; }

        protected abstract bool ShouldRebuild();
        protected abstract void Build();
        protected abstract void RefreshSubCollider(int index);

        public bool IsBindingCollider(Collider collider)
        {
            return Array.IndexOf(_subColliders, collider) >= 0;
        }

        public void OnSerializedFieldChanged()
        {
            Refresh();
        }

        #region Component Messages
        private void OnEnable()
        {
            if (!didStart)
                return;

            foreach (var collider in _subColliders)
                collider.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            foreach (var collider in _subColliders)
                collider.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            foreach (var collider in _subColliders)
            {
                if (!collider || !collider.gameObject)
                    continue;

                collider.gameObject.PlaymodeAgnosticDestroy();
            }
        }
        #endregion

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying)
                return;

            Refresh();
        }

        private void Reset()
        {
            foreach (var subCollider in GetComponentsInChildren<SubCollider>())
                subCollider.DestroyIfUnbound();
        }
#endif

        #region Private Methods
        private void RebuildIfNecessary()
        {
            if (!ShouldRebuild())
                return;

            Build();

            foreach (var collider in _subColliders)
            {
                var subCollider = collider.gameObject.AddComponent<SubCollider>();
                subCollider.colliderComponent = collider;
                subCollider.bindingCollider = this;
            }
        }

        private void Refresh()
        {
            RebuildIfNecessary();

            Bounds = new Bounds();

            for (var i = 0; i < _subColliders.Length; ++i)
            {
                _subColliders[i].isTrigger = isTrigger;
                _subColliders[i].providesContacts = providesContacts;
                _subColliders[i].material = material;
                _subColliders[i].layerOverridePriority = layerOverridePriority;
                _subColliders[i].includeLayers = includeLayers;
                _subColliders[i].excludeLayers = excludeLayers;

                RefreshSubCollider(i);

                Bounds.Encapsulate(_subColliders[i].bounds);
            }
        }
        #endregion
    }
}
