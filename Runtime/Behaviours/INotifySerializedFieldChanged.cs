using Akela.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Akela.Behaviours
{
    public interface INotifySerializedFieldChanged
    {
        void OnSerializedFieldChanged();
    }

    internal sealed class SerializedFieldChangeMonitor : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void FirstSceneLoaded()
        {
            var newGo = new GameObject("[Field Change Monitor]");
            newGo.hideFlags = HideFlags.HideAndDontSave;
            
            newGo.AddComponent<SerializedFieldChangeMonitor>();
        }

        private class MonitoredBehaviour
        {
            public INotifySerializedFieldChanged behaviour;
            public int currentHash;
        }

        private readonly List<MonitoredBehaviour> _monitoredBehaviours = new();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnEnable()
        {
            ComponentLoader<INotifySerializedFieldChanged>.OnTypeFound += RefreshMonitoredBehaviours;
        }

        private void Update()
        {
            foreach (var monitoredBehaviour in _monitoredBehaviours)
            {
                var hash = monitoredBehaviour.behaviour.GetHashCode();

                if (hash != monitoredBehaviour.currentHash)
                {
                    monitoredBehaviour.behaviour.OnSerializedFieldChanged();
                    monitoredBehaviour.currentHash = hash;
                }
            }
        }

        private void RefreshMonitoredBehaviours(IEnumerable<INotifySerializedFieldChanged> behaviours)
        {
            _monitoredBehaviours.Clear();
            _monitoredBehaviours.AddRange(behaviours.Select(x => new MonitoredBehaviour
            {
                behaviour = x,
                currentHash = x.GetHashCode()
            }));
        }
    }
}