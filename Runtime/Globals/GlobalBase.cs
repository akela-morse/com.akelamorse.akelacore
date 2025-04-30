using UnityEngine;

namespace Akela.Globals
{
    public abstract class GlobalBase<T> : ScriptableObject
    {
        [SerializeField] protected T _value;

        public T Value => _value;

        internal GlobalBase() { }
    }
}