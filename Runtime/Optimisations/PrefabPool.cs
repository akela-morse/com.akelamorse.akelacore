using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Akela.Optimisations
{
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/PrefabPoolAsset Icon.png")]
    public class PrefabPool : ScriptableObject
    {
        #region Component Fields
        [SerializeField] private PooledPrefab _prefab;
        [SerializeField] private int _maxSize = 100;
        #endregion

        private IObjectPool<PooledPrefab> _pool;

        public T Peek<T>() where T : class
        {
            return _prefab.GetComponentFromCache<T>();
        }

        public PooledPrefab Make()
        {
            return Make(_prefab.transform.position, _prefab.transform.rotation, null);
        }

        public PooledPrefab Make(Vector3 position)
        {
            return Make(position, _prefab.transform.rotation, null);
        }

        public PooledPrefab Make(Transform parent)
        {
            return Make(parent.position, parent.rotation, parent);
        }

        public PooledPrefab Make(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            CreatePoolIfNecessary();

            var pooledPrefab = _pool.Get();

            pooledPrefab.transform.SetPositionAndRotation(position, rotation);

            if (parent)
                pooledPrefab.transform.SetParent(parent, true);

            return pooledPrefab;
        }

#if UNITY_EDITOR
        public void SetPrefab(PooledPrefab prefab)
        {
            _prefab = prefab;
        }
#endif

        #region Component Fields
        private void Awake()
        {
            SceneManager.sceneUnloaded -= ChangedScene;
            SceneManager.sceneUnloaded += ChangedScene;
        }
        #endregion

        #region Private Methods
        private void ChangedScene(Scene _)
        {
            if (_pool == null)
                return;

            _pool.Clear();
        }

        private void CreatePoolIfNecessary()
        {
            if (_pool != null)
                return;

            _pool = new ObjectPool<PooledPrefab>(OnCreateObject, OnGetObject, OnReleasedObject, maxSize: _maxSize);
        }

        private PooledPrefab OnCreateObject()
        {
            var newPooledPrefab = Instantiate(_prefab);
            newPooledPrefab.LoadPool(_pool);

            return newPooledPrefab;
        }

        private static void OnGetObject(PooledPrefab pooledPrefab)
        {
            pooledPrefab.StopAllCoroutines();

            pooledPrefab.gameObject.SetActive(false); // This is to reset calls to OnDisable and OnEnable
            pooledPrefab.gameObject.SetActive(true);
        }

        private static void OnReleasedObject(PooledPrefab pooledPrefab)
        {
            pooledPrefab.gameObject.SetActive(false);
        }
        #endregion
    }
}