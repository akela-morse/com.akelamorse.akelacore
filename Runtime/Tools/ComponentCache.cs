using System.Collections.Generic;
using UnityEngine;

namespace Akela.Tools
{
    public static class ComponentCache<T> where T : class
    {
        private static readonly Dictionary<int, T> _cache = new();

        public static bool TryGet(int instanceId, out T component) => _cache.TryGetValue(instanceId, out component);
        public static bool TryGet(GameObject gameObject, out T component) => TryGet(gameObject.GetInstanceID(), out component);
        public static bool TryGet(Component source, out T component) => TryGet(source.gameObject.GetInstanceID(), out component);
        public static bool TryGet(RaycastHit hitInfo, out T component) => TryGet(hitInfo.transform.gameObject.GetInstanceID(), out component);

        public static void Initialise()
        {
            ComponentLoader<T>.OnTypeFound += LoadComponents;
        }

        private static void LoadComponents(IEnumerable<T> components)
        {
            _cache.Clear();

            foreach (var component in components)
            {
                var behaviour = component as MonoBehaviour;

#if UNITY_ASSERTIONS
                Debug.Assert(behaviour);
#endif

                var gameObject = behaviour.gameObject;
                var instanceId = gameObject.GetInstanceID();

                _cache.Add(instanceId, component);

                behaviour.destroyCancellationToken.Register(() => _cache.Remove(instanceId));
            }
        }
    }
}