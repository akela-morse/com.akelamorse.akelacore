using UnityEngine;

namespace Akela.Behaviours
{
	[CreateAssetMenu(fileName = "New Optimisation Settings", menuName = "Settings/Behaviour Optimisation Settings", order = 50)]
	public class OptimisationSettings : ScriptableObject
	{
		public bool stopExecutionWhenCulled = true;
		public float lowestTimeInterval = 0f;
		public float highestTimeInterval = 1f;
	}
}
