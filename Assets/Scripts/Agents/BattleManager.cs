using System;
using Agents.Enemies;
using Agents.Players;
using UnityEngine;

namespace Agents
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private EnemyManager enemyManager;

        private void Start()
        {
            Debug.Assert(enemyManager != null, "enemyManager is null");
            Debug.Assert(playerManager != null, "playerManager is null");
            
            //enemy 초기화
            enemyManager.ClearEnemies();
            
            //player 초기화
            playerManager.ChangePlayer(0);
        }
    }
}