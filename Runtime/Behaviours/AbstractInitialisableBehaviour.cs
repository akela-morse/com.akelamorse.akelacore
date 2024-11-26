using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Akela.Behaviours
{
    public abstract class AbstractInitialisableBehaviour : MonoBehaviour
    {
        [NonSerialized] public bool didInitialise;

        protected internal abstract void InitialiseBehaviour();
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
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            foreach (var initialisableObject in FindObjectsByType<AbstractInitialisableBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (initialisableObject.didInitialise)
                    continue;

                initialisableObject.InitialiseBehaviour();
                initialisableObject.didInitialise = true;
            }
        }
    }
}
