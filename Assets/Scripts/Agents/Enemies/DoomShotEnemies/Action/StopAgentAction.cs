using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Agents.Enemies.DoomShotEnemies.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopAgent", story: "Stop [Enemy]", category: "Action/Animation", id: "13ba8f0f663b34c2ca65867df8adcce6")]
    public partial class StopAgentAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;

        protected override Status OnStart()
        {
            if (Enemy.Value == null || Enemy.Value.NavMovement == null)
                return Status.Failure;

            Enemy.Value.NavMovement.StopImmediately();
            return Status.Success;
        }
    }
}

