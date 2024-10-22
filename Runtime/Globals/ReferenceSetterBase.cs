using UnityEngine;

namespace Akela.Globals
{
	public abstract class ReferenceSetterBase<THolder, TValue> : MonoBehaviour
		where THolder: GlobalReferenceBase<TValue>
		where TValue : Object
	{
		[SerializeField] protected THolder _variable;
		[SerializeField] protected TValue _value;

		private void Awake()
		{
			_variable.SetValue(_value);
		}
	}
}