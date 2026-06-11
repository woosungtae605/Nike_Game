using Agents.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemySetMove", story: "[Enemy] set move [direction]", category: "Action/Navigation", id: "0eedee7d32f9d7c0cc9ccca852262295")]
public partial class EnemySetMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<Vector3> Direction;

    private INavMovement _navMovement;
    protected override Status OnStart()
    {
        if (Enemy == null || Enemy.Value == null || Enemy.Value.NavMovement == null)
            return Status.Failure;

        _navMovement = Enemy.Value.NavMovement;

        _navMovement.SetDestination(Enemy.Value.transform.position + Enemy.Value.transform.TransformDirection(Direction.Value));
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return _navMovement.IsArrived ? Status.Success : Status.Running;
    }
}

