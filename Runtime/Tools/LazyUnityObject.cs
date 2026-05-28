using System;
using Object = UnityEngine.Object;

namespace Akela.Tools
{
    public struct LazyUnityObject<T> : IEquatable<LazyUnityObject<T>>, IEquatable<Object> where T : Object
    {
        private T _reference;

        private readonly Func<T> _activator;

        public T Value
        {
            get
            {
                if (!_reference)
                    _reference = _activator();

                return _reference;
            }
        }

        public LazyUnityObject(Func<T> activator)
        {
            _activator = activator;
            _reference = null;
        }

        public override int GetHashCode() => Value ? Value.GetHashCode() : int.MinValue;

        public override bool Equals(object obj) => obj is null ? !Value : obj is LazyUnityObject<T> other && Equals(other);

        public bool Equals(LazyUnityObject<T> other) => Value == other.Value;

        public bool Equals(Object other) => Value == other;


        public static implicit operator T(LazyUnityObject<T> obj) => obj.Value;
        public static implicit operator bool(LazyUnityObject<T> obj) => obj.Value;

        public static bool operator ==(LazyUnityObject<T> a, LazyUnityObject<T> b) => a.Value == b.Value;
        public static bool operator !=(LazyUnityObject<T> a, LazyUnityObject<T> b) => a.Value != b.Value;

        public static bool operator ==(LazyUnityObject<T> a, Object b) => a.Value == b;
        public static bool operator !=(LazyUnityObject<T> a, Object b) => a.Value != b;

        public static bool operator ==(LazyUnityObject<T> a, object b) => a.Equals(b);
        public static bool operator !=(LazyUnityObject<T> a, object b) => !a.Equals(b);
    }
}