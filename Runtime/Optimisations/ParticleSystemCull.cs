using System;
using UnityEngine;

namespace Akela.Optimisations
{
    [AddComponentMenu("Optimisation/Particle System Cull", 3)]
    [DisallowMultipleComponent, RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemCull : MonoBehaviour, ICullingMessageReceiver
    {
        private struct ParticleSystemState
        {
            public ParticleSystem system;
            public int initialMaxCount;
            public float initialSize;
            public float initialRateOverTime;
            public float initialRateOverDistance;

            public ParticleSystemState(ParticleSystem system)
            {
                this.system = system;

                initialMaxCount = system.main.maxParticles;
                initialSize = system.main.startSizeMultiplier;
                initialRateOverTime = system.emission.rateOverTimeMultiplier;
                initialRateOverDistance = system.emission.rateOverDistanceMultiplier;
            }
        }

        #region Component Fields
        [Tooltip("If d < x particles will be at full quality\nIf x <= d < y particles quality will progressively decrease\nIf y <= d particles will be paused")]
        [SerializeField] Vector2Int _distanceBandRange = new(1, 2);
        [Space]
        [SerializeField] bool _affectChildren = true;
        [SerializeField] bool _affectEmissionRate;
        [SerializeField] bool _affectSize;
        [Space]
        [Tooltip("How the maximum number of particles will decrease as the distance increases")]
        [SerializeField] AnimationCurve _particleCountFalloff = AnimationCurve.Linear(0f, 1f, 1f, .5f);
#if AKELA_VINSPECTOR
        [VInspector.EnableIf(nameof(_affectEmissionRate))]
#endif
        [Tooltip("How the emission rate of the Particle System will decrease as the distance increases")]
        [SerializeField] AnimationCurve _particleEmissionFalloff = AnimationCurve.Linear(0f, 1f, 1f, .25f);
#if AKELA_VINSPECTOR
        [VInspector.EndIf]
        [VInspector.EnableIf(nameof(_affectSize))]
#endif
        [Tooltip("How the Size of the particles will increase with the distance, to compensate for decreased emission")]
        [SerializeField] AnimationCurve _particleSizeCompensation = AnimationCurve.Linear(0f, 1f, 1f, 1.25f);
#if AKELA_VINSPECTOR
        [VInspector.EndIf]
#endif
        #endregion

        private ParticleSystemState[] _states;

        public void OnCullingElementInvisible() { }

        public void OnCullingElementVisible() { }

        public void OnDistanceBandChanges(int previousBand, int newBand)
        {
            if (newBand > _distanceBandRange.y) // Too far, pause the systems
            {
                _states[0].system.Pause(true);
                return;
            }
            else if (previousBand > _distanceBandRange.y) // Was too far before, resume the systems
            {
                _states[0].system.Play(true);
            }

            var quality = Mathf.InverseLerp(_distanceBandRange.x, _distanceBandRange.y, newBand);

            foreach (var state in _states)
            {
                var main = state.system.main;
                main.maxParticles = Mathf.RoundToInt(state.initialMaxCount * _particleCountFalloff.Evaluate(quality));

                if (_affectSize)
                    main.startSizeMultiplier = state.initialSize * _particleSizeCompensation.Evaluate(quality);

                if (_affectEmissionRate)
                {
                    var emission = state.system.emission;
                    emission.rateOverTimeMultiplier = state.initialRateOverTime * _particleEmissionFalloff.Evaluate(quality);
                    emission.rateOverDistanceMultiplier = state.initialRateOverDistance * _particleEmissionFalloff.Evaluate(quality);
                }
            }
        }

        #region Component Messages
        private void Awake()
        {
            if (_affectChildren)
                _states = Array.ConvertAll(GetComponentsInChildren<ParticleSystem>(), x => new ParticleSystemState(x));
            else
                _states = new[] { new ParticleSystemState(GetComponent<ParticleSystem>()) };
        }
        #endregion
    }
}