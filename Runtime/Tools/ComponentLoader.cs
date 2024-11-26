using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using Object = UnityEngine.Object;
#endif

namespace Akela.Tools
{
    public static class ComponentLoader<T> where T : class
    {
        public delegate void TypeRegistration(IEnumerable<T> instances);

        public static event TypeRegistration OnTypeFound
        {
            add
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    DelayedRegistration.AddDelayedRegistration(((Object)value.Target).GetInstanceID(), e => value(e.OfType<T>()));
                    return;
                }
#endif

                ComponentLoaderBehaviour.Main.GetInstancesOfType += e => value(e.OfType<T>());
            }
            remove { }
        }
    }

    internal sealed class ComponentLoaderBehaviour : MonoBehaviour
    {
        internal static ComponentLoaderBehaviour Main { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void FirstSceneLoaded()
        {
            var newGo = new GameObject("[Component Loader]");
            Main = newGo.AddComponent<ComponentLoaderBehaviour>();
        }

        internal event Action<MonoBehaviour[]> GetInstancesOfType;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
#if UNITY_EDITOR
            foreach (var delayedRegistration in DelayedRegistration.GetAllDelayedRegistrations())
                GetInstancesOfType += delayedRegistration;
#endif

            GetInstancesOfType?.Invoke(FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        }
    }

#if UNITY_EDITOR
    internal static class DelayedRegistration
    {
        private static readonly Dictionary<int, Action<MonoBehaviour[]>> _delayedRegistrations = new();

        public static void AddDelayedRegistration(int ownerId, Action<MonoBehaviour[]> action)
        {
            _delayedRegistrations[ownerId] = action;
        }

        public static IEnumerable<Action<MonoBehaviour[]>> GetAllDelayedRegistrations()
        {
            foreach (var registration in _delayedRegistrations.Values)
                yield return registration;

            _delayedRegistrations.Clear();
        }
    }
#endif
}