using Akela.Bridges;
using UnityEngine;

namespace Akela.Signals
{
	[AddComponentMenu("Signals/Entry Events", 2)]
	[DisallowMultipleComponent]
	public class EntryEvents : MonoBehaviour
	{
		#region Component Fields
		[SerializeField] BridgedEvent onAwake;
		[SerializeField] BridgedEvent onEnable;
		[SerializeField] BridgedEvent onStart;
		#endregion

		#region Component Messages
		private void Awake()
		{
			onAwake.Invoke();
		}

		private void OnEnable()
		{
			onEnable.Invoke();
		}

		private void Start()
		{
			onStart.Invoke();
		}
		#endregion
	}
}