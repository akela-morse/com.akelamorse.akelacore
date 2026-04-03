using System.Collections;
using System.Collections.Generic;
using Akela.Bridges;
using Akela.Tools;
using UnityEngine;
using UnityEngine.Pool;
#if AKELA_VINSPECTOR
using VInspector;
#endif

namespace Akela.Optimisations
{
    [DisallowMultipleComponent]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/PooledPrefab Icon.png")]
    [AddComponentMenu("Optimisation/Pooled Prefab", 4)]
    public class PooledPrefab : MonoBehaviour, ICullingMessageReceiver
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
        [ShowIf(nameof(_releaseToPool), ReleaseBehaviour.AfterTime)]
#endif
        [SerializeField] SerializedWaitForSeconds _releaseTime;
#if AKELA_VINSPECTOR
        [EndIf]
#endif
        [Header("Events")]
        [SerializeField] BridgedEvent<PooledPrefab> _onInstantiatedFromPool;
        [SerializeField] BridgedEvent<PooledPrefab> _onReleasedToPool;
        #endregion

        private IObjectPool<PooledPrefab> _pool;

        public void ReleaseToPool()
        {
            ReleaseNow();
        }

        public T GetComponentFromCache<T>() where T : class
        {
            for (var i = 0; i < _cachedComponents.Count; i++)
                if (typeof(T).IsAssignableFrom(_cachedComponents[i].GetType()))
                    return _cachedComponents[i] as T;

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

        #region Component Messages
#if UNITY_ASSERTIONS
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            if (_pool == null)
                Debug.LogError("A Pooled Prefab was instantiated outside of its pool! This will lead to errors.", this);
        }
#endif

        private void OnEnable()
        {
            _onInstantiatedFromPool.Invoke(this);

            if (_releaseToPool == ReleaseBehaviour.AfterTime)
                StartCoroutine(ReleaseAfterTime());
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

        #region Private
        private IEnumerator ReleaseAfterTime()
        {
            yield return _releaseTime;
            ReleaseNow();
        }

        private void ReleaseNow()
        {
            _onReleasedToPool.Invoke(this);

            _pool.Release(this);
        }
        #endregion
    }
}