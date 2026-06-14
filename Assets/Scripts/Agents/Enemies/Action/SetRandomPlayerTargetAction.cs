using Agents.Enemies;
using Agents.Players;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetRandomPlayerTarget", story: "[Enemy] set [Target] random player by [PlayerManager]", category: "Action", id: "edbaed4fb8177e9e47865f9141244470")]
public partial class SetRandomPlayerTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<PlayerManager> PlayerManager;

    protected override Status OnStart()
    {
        if (Enemy == null || Enemy.Value == null)
            return Status.Failure;

        if (PlayerManager == null || PlayerManager.Value == null)
            return Status.Failure;

        if (Target == null)
            return Status.Failure;

        Player randomPlayer = PlayerManager.Value.GetRandomPlayer();

        if (randomPlayer == null)
        {
            Target.Value = null;
            return Status.Failure;
        }

        Target.Value = randomPlayer.gameObject;
        return Status.Success;
    }
}

