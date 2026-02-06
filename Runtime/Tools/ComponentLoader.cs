using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

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

                ComponentLoaderSubscriber.GetInstancesOfType += e => value(e.OfType<T>());
            }
            remove
            {
                throw new InvalidOperationException("You don't need to unregister from OnTypeFound, it will be done automatically.");
            }
        }
    }

    internal static class ComponentLoaderSubscriber
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void FirstSceneLoaded()
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            SceneManager.sceneLoaded += SceneLoaded;
        }

        internal static event Action<MonoBehaviour[]> GetInstancesOfType;

        private static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
#if UNITY_EDITOR
            foreach (var delayedRegistration in DelayedRegistration.GetAllDelayedRegistrations())
                GetInstancesOfType += delayedRegistration;
#endif

            GetInstancesOfType?.Invoke(Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None));
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