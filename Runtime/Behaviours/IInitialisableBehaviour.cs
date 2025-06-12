using System.Collections.Generic;
using Akela.Tools;
using UnityEngine;

namespace Akela.Behaviours
{
    public interface IInitialisableBehaviour
    {
        void InitialiseBehaviour();
    }

    internal static class BehaviourInitialisationManager
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void FirstSceneLoaded()
        {
            ComponentLoader<IInitialisableBehaviour>.OnTypeFound += LoadInitialisableBehaviours;
        }

        private static void LoadInitialisableBehaviours(IEnumerable<IInitialisableBehaviour> instances)
        {
            foreach (var initialisableObject in instances)
                initialisableObject.InitialiseBehaviour();
        }
    }
}