using System;
using UnityEngine;

namespace Akela.Tools
{
    public class RunUntil : CustomYieldInstruction
    {
        private readonly Action _action;
        private readonly Func<bool> _predicate;

        public override bool keepWaiting
        {
            get
            {
                _action();

                return !_predicate();
            }
        }

        public RunUntil(Action action, Func<bool> predicate)
        {
            _action = action;
            _predicate = predicate;
        }
    }
}
