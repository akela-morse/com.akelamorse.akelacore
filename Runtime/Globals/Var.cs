using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Akela.Globals
{
	[Serializable]
	public struct Var<T>
	{
		[SerializeField] T _localValue;
		[SerializeField] GlobalBase<T> _globalValue;

		public readonly T Value => _globalValue ? _globalValue.Value : _localValue;
		public readonly bool HasValue => Value is Object O ? O : Value != null; // Thank you Unity and your stupid operator overload, very cool

		public static implicit operator T(Var<T> v) => v._globalValue ? v._globalValue.Value : v._localValue;
		public static implicit operator Var<T>(T v) => new() { _localValue = v };

		public static implicit operator bool(Var<T> v) => v.HasValue;
	}
}