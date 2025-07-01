using System.Collections.Generic;
using Akela.Tools;
using UnityEngine;

namespace Akela.Behaviours
{
    public interface ILateFixedUpdate
    {
        public bool isActiveAndEnabled { get; }

        void LateFixedUpdate();
    }

    internal sealed class LateFixedUpdateManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void FirstSceneLoaded()
        {
            var newGo = new GameObject("[LateFixedUpdate Manager]")
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            newGo.AddComponent<LateFixedUpdateManager>();
        }

        private readonly List<ILateFixedUpdate> _lateFixedUpdatees = new();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnEnable()
        {
            ComponentLoader<ILateFixedUpdate>.OnTypeFound += RefreshLateFixedUpdatees;
        }

        private void Start()
        {
            _ = LateFixedUpdate();
        }

        private void RefreshLateFixedUpdatees(IEnumerable<ILateFixedUpdate> behaviours)
        {
            _lateFixedUpdatees.Clear();
            _lateFixedUpdatees.AddRange(behaviours);
        }

        // ReSharper disable once FunctionNeverReturns
        private async Awaitable LateFixedUpdate()
        {
            for (;;)
            {
                await Awaitable.FixedUpdateAsync();

                foreach (var updatee in _lateFixedUpdatees)
                {
                    if (!updatee.isActiveAndEnabled)
                        continue;

                    updatee.LateFixedUpdate();
                }
            }
        }
    }
}