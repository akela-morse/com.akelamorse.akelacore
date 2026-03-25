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
        [SerializeField] protected float _duration;

        [NonSerialized] protected float _startTime;
        [NonSerialized] protected bool _hasStarted;

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

        public static implicit operator SerializedWaitForSeconds(float other)
        {
            return new SerializedWaitForSeconds { _duration = other };
        }

        public static implicit operator float(SerializedWaitForSeconds other)
        {
            return other._duration;
        }
    }

    [Serializable]
    public class SerializedWaitForSecondsRealtime : SerializedWaitForSeconds
    {
        public override bool keepWaiting
        {
            get
            {
                if (!_hasStarted)
                {
                    _startTime = Time.unscaledTime;
                    _hasStarted = true;
                }

                if (Time.unscaledTime - _startTime >= _duration)
                {
                    _hasStarted = false;
                    return false;
                }

                return true;
            }
        }

        public static implicit operator SerializedWaitForSecondsRealtime(float other)
        {
            return new SerializedWaitForSecondsRealtime { _duration = other };
        }

        public static implicit operator float(SerializedWaitForSecondsRealtime other)
        {
            return other._duration;
        }
    }
}