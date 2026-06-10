using System;
using Agents.Players;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Agents.Enemies.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetEnemyMoveDirection", story: "[Enemy] set direction by [PlayerManager]", category: "Action", id: "a2a5ed460fe39a02ae728eafb8933f3b")]
    public partial class SetEnemyMoveDirectionAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<PlayerManager> PlayerManager;

        protected override Status OnStart()
        {
            if (Enemy == null || Enemy.Value == null || PlayerManager == null || PlayerManager.Value == null)
            {
                return Status.Failure;
            }

            Player target = PlayerManager.Value.GetClosestPlayer(Enemy.Value.transform.position);
            if (target == null)
            {
                return Status.Failure;
            }

            Vector3 targetPosition = (target.transform.position - Enemy.Value.transform.position).normalized;
            Vector3 cross = Vector3.Cross(Enemy.Value.transform.forward, targetPosition);
            float direction = Mathf.Sign(cross.y);
            bool gotoLeft = direction < 0;
            
            Enemy.Value.SetGotoLeft(gotoLeft);
            return Status.Success;
        }
    }
}
