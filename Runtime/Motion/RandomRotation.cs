using Akela.Behaviours;
using UnityEngine;

namespace Akela.Motion
{
    [HideScriptField, DisallowMultipleComponent]
    [TickOptions(TickUpdateType.Update, TickUpdateType.LateUpdate, TickUpdateType.FixedUpdate, TickUpdateType.AnimatorMove)]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/RandomRotation Icon.png")]
    [AddComponentMenu("Motion/Random Rotation", 6)]
    public class RandomRotation : TickBehaviour
    {
        #region Component Fields
        [SerializeField] Vector3 _amplitude;
        [SerializeField] float _frequency;
        [SerializeField] float _speed;
        #endregion

        private Vector3 _initialRotation;
        private float _phase;

        #region Component Messages
        private void Awake()
        {
            _initialRotation = transform.localEulerAngles;
            _phase = Random.value;
        }

        protected override void Tick(float deltaTime)
        {
            var noiseX = Mathf.PerlinNoise(_phase * _frequency, 0f) * 2f - 1f;
            var noiseY = Mathf.PerlinNoise(_phase * _frequency, 1f) * 2f - 1f;
            var noiseZ = Mathf.PerlinNoise(_phase * _frequency, 2f) * 2f - 1f;

            var angles = Vector3.Scale(new Vector3(noiseX, noiseY, noiseZ), _amplitude);

            transform.localEulerAngles = _initialRotation + angles;

            _phase += deltaTime * _speed;
        }
        #endregion
    }
}