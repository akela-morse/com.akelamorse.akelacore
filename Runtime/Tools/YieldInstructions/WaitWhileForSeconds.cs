using System;
using UnityEngine;

namespace Akela.Tools
{
    public class WaitWhileForSeconds : CustomYieldInstruction
    {
        private readonly float _duration;
        private readonly float _startTime;
        private readonly Func<bool> _predicate;

        public override bool keepWaiting => _predicate() && (Time.time - _startTime < _duration);

        public WaitWhileForSeconds(Func<bool> predicate, float duration)
        {
            _duration = duration;
            _startTime = Time.time;
            _predicate = predicate;
        }
    }
}