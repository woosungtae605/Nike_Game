using System;
using Agents.Players;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Agents.Enemies.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetClosestPlayerTarget", story: "[Enemy] set [Target] closest player by [PlayerManager]", category: "Action/Target", id: "e176b9e2f1fb4f4e86128958d7a48706")]
    public partial class SetClosestPlayerTargetAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<PlayerManager> PlayerManager;
        [SerializeReference] public BlackboardVariable<GameObject> Target;

        protected override Status OnStart()
        {
            if (Enemy == null || Enemy.Value == null)
                return Status.Failure;

            if (PlayerManager == null || PlayerManager.Value == null)
                return Status.Failure;

            if (Target == null)
                return Status.Failure;

            Player closestPlayer = PlayerManager.Value.GetClosestPlayer(Enemy.Value.transform.position);
            if (closestPlayer == null)
            {
                Target.Value = null;
                return Status.Failure;
            }

            Target.Value = closestPlayer.gameObject;
            return Status.Success;
        }
    }
}
