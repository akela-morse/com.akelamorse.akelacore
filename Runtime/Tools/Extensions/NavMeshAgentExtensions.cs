using UnityEngine;
using UnityEngine.AI;

namespace Akela.Tools
{
    public static class NavMeshAgentExtensions
    {
        public static bool HasReachedDestination(this NavMeshAgent agent)
        {
            return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.desiredVelocity == Vector3.zero);
        }
    }
}