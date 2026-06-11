using Agents.Enemies;
using Agents.Players;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnityEngine.Rendering.DebugUI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyMoveToPlayer", story: "[Enemy] move to [Player]", category: "Action/Navigation", id: "41591b44efe304d783e52535b81aad91")]
public partial class EnemyMoveToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<PlayerManager> Player;

    private Player _player;
    private INavMovement _navMove;
    protected override Status OnStart()
    {

        _player = Player.Value.GetClosestPlayer(Enemy.Value.HitPos.position);

        Vector3 enemyPos = Enemy.Value.transform.position;
        _navMove.SetDestination(new Vector3(_player.transform.position.x, enemyPos.y, enemyPos.z));
        _navMove.Speed = Enemy.Value.EnemyDataSo.Speed * 2;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(_navMove.IsArrived)
        {
            _navMove.Speed = Enemy.Value.EnemyDataSo.Speed;
            return Status.Success;
        }

        return Status.Running;
    }
}

