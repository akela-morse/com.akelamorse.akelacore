using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace Akela.Tools
{
    public static class NavMeshAgentExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasReachedDestination(this NavMeshAgent agent)
        {
            return !agent.pathPending && agent.remainingDistance <= Mathf.Max(.001f, agent.stoppingDistance) && (!agent.hasPath || agent.desiredVelocity == Vector3.zero);
        }

        public static bool WarpPreservePath(this NavMeshAgent agent, Vector3 position)
        {
            var hasDest = !agent.HasReachedDestination();
            var dest = agent.destination;

            if (!agent.Warp(position))
                return false;

            if (hasDest)
                agent.SetDestination(dest);

            return true;
        }

        public static bool TryWarp(this NavMeshAgent agent, Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out _, 1f, new NavMeshQueryFilter { agentTypeID = agent.agentTypeID, areaMask = agent.areaMask }))
                return false;

            return agent.Warp(position);
        }

        public static bool TryWarpPreservePath(this NavMeshAgent agent, Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out _, 1f, new NavMeshQueryFilter { agentTypeID = agent.agentTypeID, areaMask = agent.areaMask }))
                return false;

            var hasDest = !agent.HasReachedDestination();
            var dest = agent.destination;

            if (!agent.Warp(position))
                return false;

            if (hasDest)
                agent.SetDestination(dest);

            return true;
        }
    }
}