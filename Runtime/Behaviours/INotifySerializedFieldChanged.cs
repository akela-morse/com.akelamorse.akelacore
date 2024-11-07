using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Akela.Behaviours
{
	public interface INotifySerializedFieldChanged
	{
		void OnSerializedFieldChanged();
	}

	internal sealed class SerializedFieldChangeMonitor : MonoBehaviour
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void FirstSceneLoaded()
		{
			var newGo = new GameObject("[Field Change Monitor]");
			newGo.AddComponent<SerializedFieldChangeMonitor>();
		}

		private class MonitoredBehaviour
		{
			public INotifySerializedFieldChanged behaviour;
			public int currentHash;
		}

		private readonly List<MonitoredBehaviour> _monitoredBehaviours = new();

		private void Awake()
		{
			DontDestroyOnLoad(this);
		}

		private void Update()
		{
			foreach (var monitoredBehaviour in _monitoredBehaviours)
			{
				var hash = monitoredBehaviour.behaviour.GetHashCode();

				if (hash != monitoredBehaviour.currentHash)
				{
					monitoredBehaviour.behaviour.OnSerializedFieldChanged();
					monitoredBehaviour.currentHash = hash;
				}
			}
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += SceneLoaded;
		}

		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= SceneLoaded;
		}

		private void SceneLoaded(Scene scene, LoadSceneMode mode)
		{
			_monitoredBehaviours.Clear();

			foreach (var monitoredBehaviour in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<INotifySerializedFieldChanged>())
			{
				_monitoredBehaviours.Add(new MonitoredBehaviour
				{
					behaviour = monitoredBehaviour,
					currentHash = monitoredBehaviour.GetHashCode()
				});
			}
		}
	}
}
