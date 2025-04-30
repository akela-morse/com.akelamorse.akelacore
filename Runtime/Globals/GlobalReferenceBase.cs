using UnityEngine;

namespace Akela.Globals
{
    public abstract class GlobalReferenceBase<T> : GlobalBase<T> where T: Object
    {
        internal void SetValue(T value) => _value = value;

        internal GlobalReferenceBase() { }
    }
}