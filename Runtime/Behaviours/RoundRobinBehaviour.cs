using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akela.Behaviours
{
    public abstract class RoundRobinBehaviour : AbstractInitialisableBehaviour
    {
        internal protected abstract void RRUpdate();

        protected internal override void InitialiseBehaviour()
        {
            var type = GetType();

            if (!RoundRobinManager.managers.TryGetValue(type, out RoundRobinManager manager))
            {
                var newObject = new GameObject($"[{type.Name} Round Robin Manager]");
                manager = newObject.AddComponent<RoundRobinManager>();

                RoundRobinManager.managers.Add(type, manager);
            }

            manager.instances.Add(this);
        }
    }

    internal sealed class RoundRobinManager : MonoBehaviour
    {
        internal static Dictionary<Type, RoundRobinManager> managers;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialise() => managers = new();

        internal List<RoundRobinBehaviour> instances = new();

        private int _currentIndex;

        private void Start()
        {
            gameObject.hideFlags = HideFlags.HideAndDontSave;

            if (managers != null)
                managers = null;
        }

        private void Update()
        {
            var current = instances[_currentIndex = (_currentIndex + 1) % instances.Count];

            if (current == null)
            {
                instances.Remove(current);
                --_currentIndex;

                return;
            }

            if (current.isActiveAndEnabled)
                current.RRUpdate();
        }
    }
}