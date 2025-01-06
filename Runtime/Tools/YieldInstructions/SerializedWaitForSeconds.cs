using System;
using UnityEngine;

namespace Akela.Tools
{
    [Serializable]
    public class SerializedWaitForSeconds : CustomYieldInstruction
    {
#if AKELA_ANIMANCER
        [Animancer.Units.Seconds]
#endif
        [SerializeField] float _duration;

        [NonSerialized] private float _startTime;
        [NonSerialized] private bool _hasStarted;

        public override bool keepWaiting
        {
            get
            {
                if (!_hasStarted)
                {
                    _startTime = Time.time;
                    _hasStarted = true;
                }

                if (Time.time - _startTime >= _duration)
                {
                    _hasStarted = false;
                    return false;
                }

                return true;
            }
        }

        public static implicit operator SerializedWaitForSeconds(float duration)
        {
            return new SerializedWaitForSeconds { _duration = duration };
        }
    }
}