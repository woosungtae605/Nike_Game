using UnityEngine;
using UnityEngine.AI;

namespace Agents.Enemies
{
    public interface INavMovement
    {
        NavMeshAgent NavAgent { get; }
        Vector3 Velocity { get; }
        float Speed { get; set; }
        bool IsStopped { get; set; }
        bool IsArrived { get;}
        
        void SetDestination(Vector3 destination);
        void StopImmediately();
    }
}