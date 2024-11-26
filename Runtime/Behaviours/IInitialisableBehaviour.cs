using System.Collections.Generic;
using Akela.Tools;
using UnityEngine;

namespace Akela.Behaviours
{
    public interface IInitialisableBehaviour
    {
        void InitialiseBehaviour();
    }

    internal sealed class BehaviourInitialisationManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void FirstSceneLoaded()
        {
            var newGo = new GameObject("[Initialisation Manager]");
            newGo.AddComponent<BehaviourInitialisationManager>();
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnEnable()
        {
            ComponentLoader<IInitialisableBehaviour>.OnTypeFound += LoadInitialisableBehaviours;
        }

        private void LoadInitialisableBehaviours(IEnumerable<IInitialisableBehaviour> instances)
        {
            foreach (var initialisableObject in instances)
                initialisableObject.InitialiseBehaviour();
        }
    }
}
