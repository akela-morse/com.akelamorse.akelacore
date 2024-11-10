using Akela.Bridges;
using UnityEngine;

namespace Akela.Signals
{
	[AddComponentMenu("Signals/Exit Events", 3)]
	[DisallowMultipleComponent]
	public class ExitEvents : MonoBehaviour
	{
		#region Component Fields
		[SerializeField] BridgedEvent onDisable;
		[SerializeField] BridgedEvent onDestroy;
		#endregion

		#region Component Messages
		private void OnDisable()
		{
			onDisable.Invoke();
		}

		private void OnDestroy()
		{
			onDestroy.Invoke();
		}
		#endregion
	}
}