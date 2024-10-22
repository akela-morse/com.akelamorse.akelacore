using System;
using UnityEngine;

namespace Akela.Globals
{
	[Serializable]
	public struct Var<T>
	{
		[SerializeField] T _localValue;
		[SerializeField] GlobalBase<T> _globalValue;

		public static implicit operator T(Var<T> v) => v._globalValue != null ? v._globalValue.Value : v._localValue;
	}
}