﻿using System;
using System.Collections.Generic;
using Akela.Bridges;
using UnityEngine;
using UnityEngine.Pool;

namespace Akela.Optimisations
{
    [DisallowMultipleComponent]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/PooledPrefab Icon.png")]
    [AddComponentMenu("Optimisation/Pooled Prefab", 4)]
    public class PooledPrefab : MonoBehaviour, ISerializationCallbackReceiver, ICullingMessageReceiver
    {
        private enum ReleaseBehaviour
        {
            AfterTime,
            OnDisabled,
            OnCulled,
            OnParticleSystemEnd,
            Manually
        }

        #region Component Fields
        [SerializeField] List<Component> _cachedComponents = new();
        [Space]
        [SerializeField] ReleaseBehaviour _releaseToPool;
#if AKELA_VINSPECTOR
        [VInspector.ShowIf(nameof(_releaseToPool), ReleaseBehaviour.AfterTime)]
#endif
#if AKELA_ANIMANCER
        [Animancer.Units.Seconds]
#endif
        [SerializeField] float _releaseTime;
#if AKELA_VINSPECTOR
        [VInspector.EndIf]
#endif
        [Header("Events")]
        [SerializeField] BridgedEvent<PooledPrefab> _onInstantiatedFromPool;
        [SerializeField] BridgedEvent<PooledPrefab> _onReleasedToPool;
        #endregion

        private readonly Dictionary<Type, Component> _cachedComponentsDictionary = new();

        private IObjectPool<PooledPrefab> _pool;

        public void ReleaseToPool()
        {
            if (_releaseToPool != ReleaseBehaviour.Manually)
                return;

            ReleaseNow();
        }

        public T GetComponentFromCache<T>() where T : Component
        {
            if (_cachedComponentsDictionary.TryGetValue(typeof(T), out var component))
                return component as T;

            return null;
        }

        internal void LoadPool(IObjectPool<PooledPrefab> pool)
        {
            _pool = pool;
        }

        #region ICullingMessageReceiver
        void ICullingMessageReceiver.OnCullingElementVisible() { }

        void ICullingMessageReceiver.OnCullingElementInvisible()
        {
            if (_releaseToPool == ReleaseBehaviour.OnCulled)
                ReleaseNow();
        }

        void ICullingMessageReceiver.OnDistanceBandChanges(int previousBand, int newBand) { }
        #endregion

        #region ISerializationCallbackReceiver
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _cachedComponents.Clear();

            foreach (var component in _cachedComponentsDictionary.Values)
                _cachedComponents.Add(component);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _cachedComponentsDictionary.Clear();

            foreach (var component in _cachedComponents)
                _cachedComponentsDictionary.Add(component.GetType(), component);
        }
        #endregion

        #region Component Messages
        private void OnEnable()
        {
            _onInstantiatedFromPool.Invoke(this);

            if (_releaseToPool == ReleaseBehaviour.AfterTime)
                Invoke(nameof(ReleaseNow), _releaseTime);
        }

        private void OnDisable()
        {
            if (_releaseToPool == ReleaseBehaviour.OnDisabled)
                ReleaseNow();
        }

        private void OnParticleSystemStopped()
        {
            if (_releaseToPool == ReleaseBehaviour.OnParticleSystemEnd)
                ReleaseNow();
        }
        #endregion

        #region Private Methods
        private void ReleaseNow()
        {
            _onReleasedToPool.Invoke(this);

            _pool.Release(this);
        }
        #endregion
    }
}