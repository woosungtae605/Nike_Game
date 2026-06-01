using Agents.Enemies;
using Agents.Players;
using UnityEngine;

namespace Agents
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private EnemyManager enemyManager;
    }
}