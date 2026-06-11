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

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }
}

