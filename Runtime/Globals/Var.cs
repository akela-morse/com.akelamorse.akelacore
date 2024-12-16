using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Akela.Globals
{
    [Serializable]
    public struct Var<T> : IEquatable<Var<T>>
    {
        [SerializeField] T _localValue;
        [SerializeField] GlobalBase<T> _globalValue;

        public readonly T Value => _globalValue ? _globalValue.Value : _localValue;
        public readonly bool HasValue => Value is Object O ? O : Value != null; // Thank you Unity and your stupid operator overload, very cool

        public static implicit operator T(Var<T> v) => v._globalValue ? v._globalValue.Value : v._localValue;
        public static implicit operator Var<T>(T v) => new() { _localValue = v };

        public static implicit operator bool(Var<T> v) => v.HasValue;

        public bool Equals(Var<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is Var<T> other && Equals(other);

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

        public static bool operator ==(Var<T> left, Var<T> right) => left.Equals(right);

        public static bool operator !=(Var<T> left, Var<T> right) => !left.Equals(right);
    }
}