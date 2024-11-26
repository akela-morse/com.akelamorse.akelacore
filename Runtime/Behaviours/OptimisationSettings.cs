using UnityEngine;

namespace Akela.Behaviours
{
    [CreateAssetMenu(fileName = "New Optimisation Settings", menuName = "Settings/Behaviour Optimisation Settings", order = 50)]
    public class OptimisationSettings : ScriptableObject
    {
        public enum CullingElementComponentSource
        {
            ThisGameObject,
            Parent,
            Children
        }

        [Tooltip("Lowest time interval in seconds between each call to OptimisedUpdate()\nWhen the CullingElement is on band 0, this will be the interval\n0 = every frame")]
        public float lowestTimeInterval = 0f;
        [Tooltip("Highest time interval in seconds between each call to OptimisedUpdate()\nWhen the CullingElement is on the last band, this will be the interval")]
        public float highestTimeInterval = 1f;
        [Space]
        public bool stopExecutionWhenCulled = true;
        [Space]
        [Tooltip("A CullingElement component is required to compute culling\nIt will be grabbed after the scene has loaded from either this GameObject, its parents, or its children")]
        public CullingElementComponentSource useCullingElementFrom = CullingElementComponentSource.Parent;
    }
}
