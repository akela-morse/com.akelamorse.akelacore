using System;
using UnityEngine;

namespace Akela.Globals
{
	[Serializable]
	public struct Var<T>
	{
		[SerializeField] T _localValue;
		[SerializeField] GlobalBase<T> _globalValue;

		public readonly T Value => _globalValue != null ? _globalValue.Value : _localValue;

		public static implicit operator T(Var<T> v) => v._globalValue != null ? v._globalValue.Value : v._localValue;
		public static implicit operator Var<T>(T v) => new() { _localValue = v };

		public static implicit operator bool(Var<T> v) => v._globalValue != null || v._localValue != null;
	}
}